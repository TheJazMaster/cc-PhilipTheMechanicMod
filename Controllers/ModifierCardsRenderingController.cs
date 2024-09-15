using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nickel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace clay.PhilipTheMechanic.Controllers
{
    [HarmonyPatch]
    public static class ModifierCardsRenderingController
    {
        private static IModCards CardsHelper => ModEntry.Instance.Helper.Content.Cards;

        internal static bool RenderingActionsOnMetalPlate = false;
        internal static bool SuppressMetalPlatingPatch = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
        public static void SupportMetalPlateRendering(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            ModifierCardsController.CalculateCardModifiers(s, c);
            int index = c.hand.IndexOf(__instance);
            if(index >= 0 && index < c.hand.Count && ShouldStickyNote(__instance, s, c, ModifierCardsController.LastCachedModifiers[index], index)) {
                __result = __result.Where((action) => action.GetIcon(s) != null && !action.disabled).ToList();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
        public static void ConditionallyReplaceArtWithMetalPlate(Card __instance, ref CardData __result, State state)
        {
            if (SuppressMetalPlatingPatch) return;

            var s = state;
            if (s.route is not Combat c) return;
            int index = c.hand.IndexOf(__instance);
            if (index < 0 || index >= c.hand.Count || !ModifierCardsController.ModifiersCurrentlyApply(s,  c, __instance)) return;

            ModifierCardsController.CalculateCardModifiers(s, c);
            List<CardModifier> modifiers = ModifierCardsController.LastCachedModifiers[index];
            if (modifiers.Count == 0) { return; }

            if (ShouldStickyNote(__instance, s, c, modifiers, index))
            {
                __result.art = ModEntry.Instance.sprites["card_art_override"];
                __result.artTint = "ffffff";
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.Render))]
        public static void RenderStickers(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            State s = fakeState ?? g.state;
            if (s.route is not Combat c) return;
            int index = c.hand.IndexOf(__instance);
            if (index < 0 || index >= c.hand.Count || !ModifierCardsController.ModifiersCurrentlyApply(s,  c, __instance)) return;

            ModifierCardsController.CalculateCardModifiers(s, c);
            List<CardModifier> modifiers = ModifierCardsController.LastCachedModifiers[index];
                
            //
            // draw stickers
            //
            if (!ShouldStickyNote(__instance, s, c, modifiers, index))
            {
                // sticker goes at (50, 8) - 0.5*sticker.dimensions
                //var DEG_60 = 1.0472;
                Vec vec = posOverride ?? __instance.pos;
                Rect rect = (__instance.GetScreenRect() + vec + new Vec(0.0, __instance.hoverAnim * -2.0 + Mutil.Parabola(__instance.flipAnim) * -10.0 + Mutil.Parabola(Math.Abs(__instance.flopAnim)) * -10.0 * Math.Sign(__instance.flopAnim))).round();
                Rect value = rect;
                if (overrideWidth.HasValue)
                {
                    rect.w = overrideWidth.Value;
                }

                Box box = g.Push(null, rect);
                Vec vec2 = box.rect.xy + new Vec(0.0, 1.0);
                var DEG_30 = 0.5236;
                int stickerCount = 0;
                double stickerOriginX = 50 - 7.5; // sticker radius is 7.5, center should be at 50, relative to card pos
                double stickerOriginY = 8 - 7.5 + 5;

                var stickers = modifiers
                    .Select(modifier => modifier.GetSticker(s))
                    .Where(sticker => sticker != null)
                    .Select(sticker => sticker!.Value);

                foreach (var sticker in stickers)
                {
                    var seed = __instance.uuid + stickerCount * 700;
                    var xRandOff = UuidToRandRange(seed, -6, 6);
                    var yRandOff = UuidToRandRange(seed + 37, -3, 10);
                    var randRotation = UuidToRandRange(seed, -DEG_30, DEG_30);
                    Draw.Sprite(sticker, vec2.x + stickerOriginX + xRandOff, vec2.y + stickerOriginY + yRandOff, rotation: randRotation, originPx: new Vec() { x = 7, y = 7 });
                    stickerCount++;
                }
                g.Pop();
            }

        }

        public static double UuidToRandRange(double uuid, double min, double max)
        {
            //return (0.5f * Math.Sin(uuid) + 0.5f) * (max-min) + min;
            return (Math.Abs(uuid / 100.0) % Math.Abs(max - min)) + min;
        }

        private static bool recursion = false;

        // Cache multipartness per card forever
        enum MultiPartCardPhase {
            BEFORE_DUMMY = 1, DUMMY = 2, AFTER_DUMMY = 3
        }
        private static readonly Dictionary<(int, Upgrade), bool> multiPartCard = [];
        private static bool IsMultiPartCard(Card card, State s, Combat c) {
            if (IsFloppable(card, s, c)) return true;
            if (multiPartCard.TryGetValue((card.uuid, card.upgrade), out bool value)) return value;

            if (recursion) return false;
            recursion = true;
            List<CardAction> actions = card.GetActions(s, c);
            recursion = false;

            MultiPartCardPhase cnt = MultiPartCardPhase.BEFORE_DUMMY;
            for (int i = 0; i < actions.Count; i++) {
                if (cnt == MultiPartCardPhase.BEFORE_DUMMY && actions[i] is not ADummyAction) {
                    cnt = MultiPartCardPhase.DUMMY;
                } else if (cnt == MultiPartCardPhase.DUMMY && actions[i] is ADummyAction) {
                    cnt = MultiPartCardPhase.AFTER_DUMMY;
                } else if (cnt == MultiPartCardPhase.AFTER_DUMMY && actions[i] is not ADummyAction) {
                    multiPartCard.Add((card.uuid, card.upgrade), true);
                    return true;
                }
            }
            multiPartCard.Add((card.uuid, card.upgrade), false);
            return false;
        }

        // Cache whether a card is a description card forever
        private static readonly Dictionary<(int, Upgrade), bool> descriptionCard = [];
        private static bool IsDescriptionCard(Card card, State s, Combat c) {
            if (descriptionCard.TryGetValue((card.uuid, card.upgrade), out bool value)) return value;

            if (recursion) return false;
            recursion = true;
            CardData data = card.GetData(s);
            recursion = false;

            bool hasDescription = data.description != null;
            descriptionCard.Add((card.uuid, card.upgrade), hasDescription);
            return hasDescription;
        }

        // Cache whether a card is floppable forever
        private static readonly Dictionary<(int, Upgrade), bool> floppableCard = [];
        private static bool IsFloppable(Card card, State s, Combat c) {
            if (floppableCard.TryGetValue((card.uuid, card.upgrade), out bool value)) return value;

            if (recursion) return false;
            recursion = true;
            CardData data = card.GetData(s);
            recursion = false;

            bool isFloppable = data.floppable;
            floppableCard.Add((card.uuid, card.upgrade), isFloppable);
            return isFloppable;
        }

        // TODO: Decouple this (getactions) from getdata
        public static bool ShouldStickyNote(Card card, State s, Combat c, List<CardModifier> modifiers, int handIndex)
        {
            if (modifiers.Where(m => m.MandatesStickyNote()).Any()) return true;

            SuppressMetalPlatingPatch = true;
            bool isDescriptionCard = IsDescriptionCard(card, s, c);
            SuppressMetalPlatingPatch = false;
            if (isDescriptionCard) return false;

            bool isMultiPartCard = IsMultiPartCard(card, s, c);
            bool hasStickyNoteRequest = modifiers.Where(m => m.RequestsStickyNote()).Any();

            return isMultiPartCard && hasStickyNoteRequest;
        }
    }
}
