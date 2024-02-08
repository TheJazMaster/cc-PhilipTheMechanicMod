using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace clay.PhilipTheMechanic.Controllers
{
    [HarmonyPatch]
    public static class ModifierCardsController
    {
        // patch get actions and stuff like that
        private static bool SuppressActionMods = false;

        public static List<ICardModifier> GetCardModifiers(Card target, State s, Combat c)
        {
            List<ICardModifier> modifiers = new List<ICardModifier>();

            foreach (Card card in c.hand)
            {
                SuppressActionMods = true;
                var actions = card.GetActionsOverridden(s, c);
                SuppressActionMods = false;

                foreach (CardAction action in actions)
                {
                    if (action is not AModifierWrapper wrapper) continue;
                    if (!wrapper.IsTargeting(target, card, c)) continue;

                    modifiers.AddRange(wrapper.modifiers);
                }
            }

            // TODO: sort CardModifiers by their wrappers' priority
            return modifiers;
        }

        //
        // flip directional modifiers
        //

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
        public static void FlipModifierWrappers(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (!__instance.flipped) return;
            foreach (CardAction action in __result)
            {
                if (action is ADirectionalCardModifierWrapper dw) dw.left = !dw.left;
                if (action is AWholeHandDirectionalCardsModifierWrapper whdw) whdw.left = !whdw.left;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
        public static void FlippableModifierWrappers(Card __instance, ref CardData __result, State state)
        {
            if (state.route is not Combat c) return;

            if (!__result.flippable && state.ship.Get(Status.tableFlip) > 0)
            {
                var actions = __instance.GetActions(state, c);
                foreach (CardAction action in actions)
                {
                    if (action is ADirectionalCardModifierWrapper || action is AWholeHandDirectionalCardsModifierWrapper)
                    {
                        __result.flippable = true;
                        return;
                    }
                }
            }
        }

        //
        // apply modifiers
        //

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
        public static void ApplyActionModifiers(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (SuppressActionMods) return;
            if (s.route is Combat combat && combat.routeOverride != null && !combat.eyeballPeek) { return; }
            if (s.route is not Combat) { return; }

            List<CardAction> overridenCardActions = __result;
            var modifiers = GetCardModifiers(__instance, s, c);
            foreach (ICardModifier modifier in modifiers)
            {
                overridenCardActions = modifier.TransformActions(overridenCardActions, s, c);
            }

            // TODO: this will break for modded characters
            // also TODO: make this work with nickel
            //try
            //{
            //    string dialogueSelector = $".{Enum.GetName<Deck>(__instance.GetMeta().deck)}Card_ModifiedByPhilip";
            //    overridenCardActions.Add(new ADummyAction() { dialogueSelector = dialogueSelector });
            //    overridenCardActions.Insert(0, new ADummyAction() { });
            //}
            //catch (Exception e) { }

            // TODO: sticky notes
            //if (!StickyNoteHack)
            //{
            //    __result = overridenCardActions;
            //}
            //else
            //{
            //    // we're trying to draw only the active actions, with icons rn
            //    __result = overridenCardActions.Where((action) => action.GetIcon(s) != null && !action.disabled).ToList();
            //}
            __result = overridenCardActions;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
        public static void ApplyDataModifiers(Card __instance, ref CardData __result, State state)
        {
            // change how this is handled
            //if (state.ship.Get(Enum.Parse<Status>("tableFlip")) > 0 && __instance is ModifierCard)
            //{
            //    __result.flippable = true;
            //}

            if (state.route is not Combat c) { return; }
            if (c.routeOverride != null && !c.eyeballPeek) { return; }

            CardData data = __result;
            var modifiers = GetCardModifiers(__instance, state, c);
            foreach (ICardModifier modifier in modifiers)
            {
                data = modifier.TransformData(data, state, c);
            }

            __result = data;
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.Render))]
        public static void RenderStickersAndStickyNotes(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            SuppressActionMods = false;
            State state = fakeState ?? g.state;

            if (state.route is not Combat c) { return; } 
            if (c.routeOverride != null && !c.eyeballPeek) { return; }
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

            var actions = __instance.GetActionsOverridden(state, c);
            // TODO: reimplement sticky notes
            //if (ShouldStickyNote(__instance, actions, state))
            //{
            //    if (actions.Where((action) => action.GetIcon(state) != null && !action.disabled).Count() <= 3)
            //    {
            //        Draw.Sprite((Spr)MainManifest.sprites["floppable_fix_sticky_note"].Id, vec2.x, vec2.y);
            //    }
            //    else
            //    {
            //        Draw.Sprite((Spr)MainManifest.sprites["floppable_fix_index_card"].Id, vec2.x, vec2.y);
            //    }

            //    StickyNoteHack = true;
            //    __instance.MakeAllActionIcons(g, g.state);
            //    StickyNoteHack = false;
            //}

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

            var stickers = GetCardModifiers(__instance, state, c)
                .Select(modifier => modifier.GetSticker(state))
                .Where(sticker => sticker != null)
                .Select(sticker => sticker!.Value);

            foreach (var sticker in stickers)
            {
                var seed = __instance.uuid + stickerCount * 700;
                var xRandOff = uuidToRandRange(seed, -6, 6);
                var yRandOff = uuidToRandRange(seed + 37, -3, 10);
                var randRotation = uuidToRandRange(seed, -DEG_30, DEG_30);
                Draw.Sprite(sticker, vec2.x + stickerOriginX + xRandOff, vec2.y + stickerOriginY + yRandOff, rotation: randRotation, originPx: new Vec() { x = 7, y = 7 });
                //MainManifest.Instance?.Logger?.LogInformation($"seed={seed} xRandOff={xRandOff} yRandOff={yRandOff} rotation={randRotation}");
                stickerCount++;
            }

            g.Pop();
        }

        public static double uuidToRandRange(double uuid, double min, double max)
        {
            //return (0.5f * Math.Sin(uuid) + 0.5f) * (max-min) + min;
            return (Math.Abs(uuid / 100.0) % Math.Abs(max - min)) + min;
        }

        public static void HandleFlimsyModifiers(Card playedCard, State s, Combat c, int handPosition)
        {
            bool readdedPlayedCard = false;
            if (c.hand.Count <= handPosition || c.hand[handPosition] != playedCard)
            {
                c.hand.Insert(handPosition, playedCard);
                readdedPlayedCard = true;
            }

            List<Card> cardsToDiscard = new();
            foreach (Card card in c.hand)
            {
                SuppressActionMods = true;
                var actions = card.GetActionsOverridden(s, c);
                SuppressActionMods = false;

                foreach (CardAction action in actions)
                {
                    if (action is not AModifierWrapper wrapper) continue;
                    if (!wrapper.isFlimsy) continue;
                    if (!wrapper.IsTargeting(playedCard, card, c)) continue;

                    cardsToDiscard.Add(card);
                    break;
                }
            }

            if (readdedPlayedCard) c.hand.RemoveAt(handPosition);
            foreach (Card card in cardsToDiscard)
            {
                s.RemoveCardFromWhereverItIs(card.uuid);
                c.SendCardToDiscard(s, card);
            }
        }
    }
}
