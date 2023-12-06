using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    [HarmonyPatch(typeof(Card))]
    public static class ModifiedCardsRegistry
    {
        public enum ModifierPriority
        {
            ABSOLUTE_LAST,  // typically for cards that erase all actions
            LAST,           // typically for cards that modify existing actions
            LATE,
            STANDARD,       // typically for cards that modify energy costs or CardData
            EARLY,
            FIRST,          // typically for cards that add new actions
            ABSOLUTE_FIRST
        }
        public struct CardModRegistration
        {
            public ModifierPriority priority;
            public Card from;
            public CardActionsModification? actionsModification;
            public CardEnergyModification? energyModification;
            public CardDataModification? dataModification;
            public List<Spr>? stickers;
        }

        public delegate List<CardAction> CardActionsModification(List<CardAction> cardActions, State s);
        public delegate int CardEnergyModification(int originalEnergy);
        public delegate CardData CardDataModification(CardData cardData);
        public static Dictionary<int, List<CardModRegistration>> cardMods = new();


        private static bool StickyNoteHack = false;


        public static void RegisterMod(
            Card self, 
            Card modifiedCard,
            ModifierPriority priority,
            CardActionsModification? actionsModification = null, 
            CardEnergyModification? energyModification = null, 
            CardDataModification? dataModification = null,
            List<Spr>? stickers = null
        ) {
            //MainManifest.Instance.Logger.LogInformation($"Card modification being registered for {modifiedCard.uuid}:`{modifiedCard.GetFullDisplayName()}` by {self.uuid}:`{self.GetFullDisplayName()}`");

            if (!cardMods.ContainsKey(modifiedCard.uuid)) { cardMods[modifiedCard.uuid] = new List<CardModRegistration>(); }
            cardMods[modifiedCard.uuid].Add(new CardModRegistration() { 
                priority = priority,
                from = self, 
                actionsModification = actionsModification,
                energyModification = energyModification,
                dataModification = dataModification,
                stickers = stickers
            });
            cardMods[modifiedCard.uuid].Sort((a, b) => b.priority.CompareTo(a.priority));
        }

        public static void DeregisterMods(Card moddingCard, Card moddedCard)
        {
            if (!cardMods.ContainsKey(moddedCard.uuid)) { return; }
            MainManifest.Instance.Logger.LogInformation($"Deregistering card modification for {moddedCard.uuid}:`{moddedCard.GetFullDisplayName()}` by {moddingCard.uuid}:`{moddingCard.GetFullDisplayName()}`");

            for (int i = 0; i < cardMods[moddedCard.uuid].Count; i++)
            {
                if (cardMods[moddedCard.uuid][i].from == moddingCard)
                {
                    cardMods[moddedCard.uuid].RemoveAt(i);
                    i--;
                }
            }

            if (cardMods[moddedCard.uuid].Count() <= 0)
            {
                cardMods.Remove(moddedCard.uuid);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.AfterWasPlayed))]
        public static void HarmonyPostfix_Card_AfterWasPlayed(Card __instance, State state, Combat c)
        {
            if (!cardMods.ContainsKey(__instance.uuid)) return;

            // Handle flimsy modifier cards (discard when a card they modify is played)
            List<CardModRegistration> cardModsEphemeral = new(cardMods[__instance.uuid]);
            foreach (var registration in cardModsEphemeral)
            {
                if (registration.from is ModifierCard mc && mc.IsFlimsy())
                {
                    var mcData = mc.GetDataWithOverrides(state);
                    bool infinite = mcData.infinite && !mcData.exhaust;

                    if (mcData.exhaust)
                    {
                        c.hand.Remove(mc);
                        mc.OnDiscard(state, c);

                        mc.ExhaustFX();
                        c.SendCardToExhaust(state, mc);
                        c.QueueImmediate(new ADelay());
                    }
                    else if (mcData.recycle && !infinite)
                    {
                        c.hand.Remove(mc);
                        mc.OnDiscard(state, c);
                        state.SendCardToDeck(mc);
                    }
                    else if (!infinite)
                    {
                        c.hand.Remove(mc);
                        mc.OnDiscard(state, c);
                        c.SendCardToDiscard(state, mc);
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetActionsOverridden))]
        public static void HarmonyPostfix_Card_GetActions(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (s.route is Combat combat && combat.routeOverride != null && !combat.eyeballPeek) { return; }
            if (s.route is not Combat) { return; } // should never hit this case
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }
            if (cardMods[__instance.uuid].Count <= 0) { return; }

            List<CardAction> overridenCardActions = __result;
            foreach (var registration in cardMods[__instance.uuid])
            {
                if (registration.actionsModification == null) continue;
                overridenCardActions = registration.actionsModification(overridenCardActions, s);
            }

            if (!StickyNoteHack)
            {
                __result = overridenCardActions;
            }
            else
            {
                // we're trying to draw only the active actions, with icons rn
                __result = overridenCardActions.Where((action) => action.GetIcon(s) != null && !action.disabled).ToList();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetCurrentCost))]
        public static void HarmonyPostfix_Card_GetCurrentCost(Card __instance, ref int __result, State s)
        {
            if (s.route is Combat c && c.routeOverride != null && !c.eyeballPeek) { return; }
            if (s.route is not Combat) { return; } // should never hit this case
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }

            int cost = __result;
            foreach (var registration in cardMods[__instance.uuid])
            {
                if (registration.energyModification == null) continue;

                //MainManifest.Instance?.Logger?.LogInformation($"Applying energy modification for {__instance.uuid}:`{__instance.GetFullDisplayName()}` from {registration.from.uuid}:`{registration.from.GetFullDisplayName()}`");
                cost = registration.energyModification(cost);
            }

            __result = cost;
        }


        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetDataWithOverrides))]
        public static void HarmonyPostfix_Card_GetData(Card __instance, ref CardData __result, State state)
        {
            if (state.ship.Get(Enum.Parse<Status>("tableFlip")) > 0 && __instance is ModifierCard)
            {
                __result.flippable = true;
            }

            if (state.route is Combat c && c.routeOverride != null && !c.eyeballPeek) { return; }
            if (state.route is not Combat) { return; } // should never hit this case
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }

            CardData data = __result;
            foreach (var registration in cardMods[__instance.uuid])
            {
                if (registration.dataModification == null) continue;

                //MainManifest.Instance?.Logger?.LogInformation($"Applying data modification for {__instance.uuid}:`{__instance.GetFullDisplayName()}` from {registration.from.uuid}:`{registration.from.GetFullDisplayName()}`");
                data = registration.dataModification(data);
            }

            __result = data;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.Render))]
        public static void HarmonyPostfix_Card_Render(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            State state = fakeState ?? g.state;

            if (state.route is Combat c && c.routeOverride != null && !c.eyeballPeek) { return; }
            if (state.route is not Combat) { return; } // should never hit this case
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }
            if (__instance.drawAnim != 1) { return; }


            Vec vec = posOverride ?? __instance.pos;
            Rect rect = (__instance.GetScreenRect() + vec + new Vec(0.0, __instance.hoverAnim * -2.0 + Mutil.Parabola(__instance.flipAnim) * -10.0 + Mutil.Parabola(Math.Abs(__instance.flopAnim)) * -10.0 * Math.Sign(__instance.flopAnim))).round();
            Rect value = rect;
            if (overrideWidth.HasValue)
            {
                rect.w = overrideWidth.Value;
            }

            Box box = g.Push(null, rect);
            Vec vec2 = box.rect.xy + new Vec(0.0, 1.0);

            //Draw.Sprite((Spr)MainManifest.sprites["icon_screw"].Id, vec2.x + 46, vec2.y + 19);
            //Draw.Sprite((Spr)MainManifest.sprites["icon_screw"].Id, vec2.x + 4,  vec2.y + 69);

            //
            // draw index card / sticky note fix for floppables
            //

            var actions = __instance.GetActionsOverridden(state, state.route as Combat);
            if (ShouldStickyNote(__instance, actions, state))
            {
                if (actions.Where((action) => action.GetIcon(state) != null && !action.disabled).Count() <= 3)
                {
                    Draw.Sprite((Spr)MainManifest.sprites["floppable_fix_sticky_note"].Id, vec2.x, vec2.y);
                }
                else
                {
                    Draw.Sprite((Spr)MainManifest.sprites["floppable_fix_index_card"].Id, vec2.x, vec2.y);
                }

                StickyNoteHack = true;
                __instance.MakeAllActionIcons(g, g.state);
                StickyNoteHack = false;
            }

            //
            // draw stickers
            //

            // sticker goes at (50, 8) - 0.5*sticker.dimensions
            //var DEG_60 = 1.0472;
            //MainManifest.Instance?.Logger?.LogInformation($"Drawing stickers on {__instance.uuid}:`{__instance.GetFullDisplayName()}`");
            var DEG_30 = 0.5236;
            int stickerCount = 0;
            double stickerOriginX = 50 - 7.5; // sticker radius is 7.5, center should be at 50, relative to card pos
            double stickerOriginY = 8 - 7.5 + 5;
            foreach (var registration in cardMods[__instance.uuid])
            {
                if (registration.stickers == null) continue;
                foreach (var sticker in registration.stickers)
                {
                    var seed = __instance.uuid + stickerCount*700;
                    var xRandOff = uuidToRandRange(seed, -6, 6);
                    var yRandOff = uuidToRandRange(seed + 37, -3, 10);
                    var randRotation = uuidToRandRange(seed, -DEG_30, DEG_30);
                    Draw.Sprite(sticker, vec2.x + stickerOriginX + xRandOff, vec2.y + stickerOriginY + yRandOff, rotation: randRotation, originPx: new Vec() { x = 7, y = 7 });
                    //MainManifest.Instance?.Logger?.LogInformation($"seed={seed} xRandOff={xRandOff} yRandOff={yRandOff} rotation={randRotation}");
                    stickerCount++;
                }
            }

            g.Pop();
        }

        public static bool ShouldStickyNote(Card card, List<CardAction> actions, State s)
        {
            bool foundIcon = false;
            bool foundNoIcon = false;
            foreach (CardAction action in actions)
            {
                bool hasIcon = action.GetIcon(s) != null;
                if (hasIcon) 
                {
                    if (foundIcon && foundNoIcon) return true;
                    foundIcon = true;
                }
                else
                {
                    foundNoIcon = true;
                }
            }
            return false;
        }

        public static double uuidToRandRange(double uuid, double min, double max)
        {
            //return (0.5f * Math.Sin(uuid) + 0.5f) * (max-min) + min;
            return (Math.Abs(uuid / 100.0) % Math.Abs(max - min)) + min;
        }

        //[HarmonyPostfix]
        //[HarmonyPatch(nameof(Card.GetFullDisplayName))]
        //public static void TEMP(Card __instance, ref string __result)
        //{
        //    if (!cardMods.ContainsKey(__instance.uuid)) { return; }

        //    __result = "MODIFIED " + __result;
        //}
    }
}
