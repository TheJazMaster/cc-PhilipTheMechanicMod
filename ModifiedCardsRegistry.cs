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
        public struct CardModRegistration
        {
            public Card from;
            public CardActionsModification? actionsModification;
            public CardEnergyModification? energyModification;
        }

        public delegate List<CardAction> CardActionsModification(List<CardAction> cardActions);
        public delegate int CardEnergyModification(int originalEnergy);
        public static Dictionary<int, List<CardModRegistration>> cardMods = new();

        public static void RegisterMod(Card self, Card modifiedCard, CardActionsModification? actionsModification, CardEnergyModification? energyModification)
        {
            MainManifest.Instance.Logger.LogInformation($"Card modification being registered for {modifiedCard.uuid}:`{modifiedCard.GetFullDisplayName()}` by {self.uuid}:`{self.GetFullDisplayName()}`");

            if (!cardMods.ContainsKey(modifiedCard.uuid)) { cardMods[modifiedCard.uuid] = new List<CardModRegistration>(); }
            cardMods[modifiedCard.uuid].Add(new CardModRegistration() { 
                from = self, 
                actionsModification = actionsModification,
                energyModification = energyModification
            });
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
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetActionsOverridden))]
        public static void HarmonyPostfix_Card_GetActions(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (s.route is Combat combat && combat.routeOverride != null && !combat.eyeballPeek) { return; }
            if (s.route is not Combat) { return; } // should never hit this case
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }

            List<CardAction> overridenCardActions = __result;
            foreach (var registration in cardMods[__instance.uuid])
            {
                if (registration.actionsModification == null) continue;
                overridenCardActions = registration.actionsModification(overridenCardActions);
            }

            __result = overridenCardActions;
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

                MainManifest.Instance?.Logger?.LogInformation($"Applying energy modification for {__instance.uuid}:`{__instance.GetFullDisplayName()}` from {registration.from.uuid}:`{registration.from.GetFullDisplayName()}`");
                cost = registration.energyModification(cost);
            }

            __result = cost;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.Render))]
        public static void HarmonyPostfix_Card_Render(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            State state = fakeState ?? g.state;

            if (state.route is Combat c && c.routeOverride != null && !c.eyeballPeek) { return; }
            if (state.route is not Combat) { return; } // should never hit this case
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }

            Vec vec = posOverride ?? __instance.pos;
            Rect rect = (__instance.GetScreenRect() + vec + new Vec(0.0, __instance.hoverAnim * -2.0 + Mutil.Parabola(__instance.flipAnim) * -10.0 + Mutil.Parabola(Math.Abs(__instance.flopAnim)) * -10.0 * (double)Math.Sign(__instance.flopAnim))).round();
            Rect value = rect;
            if (overrideWidth.HasValue)
            {
                rect.w = overrideWidth.Value;
            }

            int currentCost = __instance.GetCurrentCost(state);
            bool flag = !(state.route is Combat combat) || combat.energy >= currentCost;
            bool flag2 = forceIsInteractible ?? (__instance.drawAnim >= 1.0);
            Box box = g.Push(flag2 ? new UIKey?(keyOverride ?? __instance.UIKey()) : null, rect, value, depth: (!ignoreHover && __instance.isForeground) ? 1 : 0, onMouseDown: flag2 ? onMouseDown : null, onMouseDownRight: flag2 ? onMouseDownRight : null, onInputPhase: ignoreHover ? null : onInputPhase, onAfterUI: __instance, autoFocus: flag2 && autoFocus, noHoverSound: false, gamepadUntargetable: false, rightHint: rightHint, leftHint: leftHint, upHint: upHint, downHint: downHint, reticleMode: (isInCombatHand && g.state.hideCardTooltips) ? ReticleMode.QuadCardHint : ReticleMode.Quad);
            Vec vec2 = box.rect.xy + new Vec(0.0, 1.0);

            Draw.Sprite((Spr)MainManifest.sprites["icon_screw"].Id, vec2.x + 46, vec2.y + 19);
            Draw.Sprite((Spr)MainManifest.sprites["icon_screw"].Id, vec2.x + 4,  vec2.y + 69);

            // sticker goes at (50, 8) - 0.5*sticker.dimensions
            //var DEG_60 = 1.0472;
            var DEG_30 = 0.5236;
            var xRandOff = uuidToRandRange(__instance.uuid, -3, 3);
            var yRandOff = uuidToRandRange(__instance.uuid+37, -3, 3);
            Draw.Sprite((Spr)MainManifest.sprites["icon_2x_sticker"].Id, vec2.x + 50-7 + xRandOff,  vec2.y + 8-7 + yRandOff, rotation: uuidToRandRange(__instance.uuid, 0, DEG_30), originPx: new Vec() { x=7, y=7 });

            g.Pop();
        }

        public static double uuidToRandRange(double uuid, double min, double max)
        {
            //return (0.5f * Math.Sin(uuid) + 0.5f) * (max-min) + min;
            return (uuid / 10.0) * (max-min) + min;
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
