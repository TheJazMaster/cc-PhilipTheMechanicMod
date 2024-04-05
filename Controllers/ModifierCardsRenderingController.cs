using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace clay.PhilipTheMechanic.Controllers
{
    [HarmonyPatch]
    public static class ModifierCardsRenderingController
    {
        //
        // Move below to new class ModifierRendererController?
        //

        internal static bool RenderingActionsOnMetalPlate = false;
        internal static bool SuppressMetalPlatingPatch = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
        public static void SupportMetalPlateRendering(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (!RenderingActionsOnMetalPlate) return;

            // we're trying to draw only the active actions with icons rn
            __result = __result.Where((action) => action.GetIcon(s) != null && !action.disabled).ToList();
        }

        // stack overflows from infinite recursion that happens for no reason
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
        public static void ConditionallyReplaceArtWithMetalPlate(Card __instance, ref CardData __result, State state)
        {
            if (SuppressMetalPlatingPatch) return;
            if (__instance == null) return;

            var s = state;
            if (!ModifierCardsController.ModifiersCurrentlyApply(state, __instance)) return;
            Combat c = (s.route as Combat)!;

            ModifierCardsController.SuppressActionMods = true;
            var modifiers = ModifierCardsController.GetCardModifiers(__instance, s, c);
            ModifierCardsController.SuppressActionMods = false;
            if (modifiers.Count == 0) { return; }

            ModifierCardsController.SuppressActionMods = true;
            var actions = __instance.GetActionsOverridden(s, c);
            ModifierCardsController.SuppressActionMods = false;

            if (ShouldStickyNote(__instance, s, actions, modifiers))
            {
                __result.art = ModEntry.Instance.sprites["card_art_override"].Sprite;
                __result.artTint = "ffffff";
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.Render))]
        public static void RenderStickers(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            ModifierCardsController.SuppressActionMods = false;
            State state = fakeState ?? g.state;

            var s = state;
            if (!ModifierCardsController.ModifiersCurrentlyApply(state, __instance)) return;
            Combat c = (s.route as Combat)!;


            Vec vec = posOverride ?? __instance.pos;
            Rect rect = (__instance.GetScreenRect() + vec + new Vec(0.0, __instance.hoverAnim * -2.0 + Mutil.Parabola(__instance.flipAnim) * -10.0 + Mutil.Parabola(Math.Abs(__instance.flopAnim)) * -10.0 * Math.Sign(__instance.flopAnim))).round();
            Rect value = rect;
            if (overrideWidth.HasValue)
            {
                rect.w = overrideWidth.Value;
            }

            Box box = g.Push(null, rect);
            Vec vec2 = box.rect.xy + new Vec(0.0, 1.0);

            //Draw.Sprite(ModEntry.Instance.sprites["icon_screw"].Sprite, vec2.x + 46, vec2.y + 19);
            //Draw.Sprite(ModEntry.Instance.sprites["icon_screw"].Sprite, vec2.x + 4,  vec2.y + 69);

            var modifiers = ModifierCardsController.GetCardModifiers(__instance, state, c);


            var actions = __instance.GetActionsOverridden(state, c);
            if(ShouldStickyNote(__instance, state, actions, modifiers))
            {
                RenderingActionsOnMetalPlate = true;
                __instance.MakeAllActionIcons(g, g.state);
                RenderingActionsOnMetalPlate = false;
            }
            else
            {
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

                var stickers = modifiers
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
            }

            g.Pop();
        }

        public static double uuidToRandRange(double uuid, double min, double max)
        {
            //return (0.5f * Math.Sin(uuid) + 0.5f) * (max-min) + min;
            return (Math.Abs(uuid / 100.0) % Math.Abs(max - min)) + min;
        }

        public static bool ShouldStickyNote(Card card, State s, List<CardAction> actions, List<ICardModifier> modifiers)
        {
            SuppressMetalPlatingPatch = true;
            var description = card.GetDataWithOverrides(s).description;
            SuppressMetalPlatingPatch = false;
            if (description != null) return false;

            if (modifiers.Where(m => m.MandatesStickyNote()).Any()) return true;

            var hasDisabledAction = actions.Where(a => a.disabled).Any();
            var hasStickyNoteRequest = modifiers.Where(m => m.RequestsStickyNote()).Any();

            return hasDisabledAction && hasStickyNoteRequest;
        }
    }
}
