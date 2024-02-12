﻿using clay.PhilipTheMechanic.Actions.CardModifiers;
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
    public static class ModifierCardsController
    {
        // patch get actions and stuff like that
        internal static bool SuppressActionMods = false;

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
                overridenCardActions = modifier.TransformActions(overridenCardActions, s, c, __instance);
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

            var s = state;
            if (!ModifiersCurrentlyApply(state, __instance)) return;
            Combat c = (s.route as Combat)!;


            CardData data = __result;
            var modifiers = GetCardModifiers(__instance, state, c);
            foreach (ICardModifier modifier in modifiers)
            {
                data = modifier.TransformData(data, state, c);
            }

            __result = data;
        }


        //
        // Move below to new class ModifierRendererController?
        //

        internal static bool RenderingActionsOnStickyNote = false;
        internal static bool SuppressMetalPlatingPatch = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
        public static void SupportStickyNoteRendering(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (!RenderingActionsOnStickyNote) return;

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
            if (!ModifiersCurrentlyApply(state, __instance)) return;
            Combat c = (s.route as Combat)!;

            SuppressActionMods = true;
            var modifiers = GetCardModifiers(__instance, s, c);
            SuppressActionMods = false;
            if (modifiers.Count == 0) { return; }

            SuppressActionMods = true;
            var actions = __instance.GetActionsOverridden(s, c);
            SuppressActionMods = false;

            if (ShouldStickyNote(__instance, s, actions, modifiers))
            {
                __result.art = ModEntry.Instance.sprites["card_art_override"].Sprite;
                __result.artTint = "ffffff";
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.Render))]
        public static void RenderStickersAndStickyNotes(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            SuppressActionMods = false;
            State state = fakeState ?? g.state;

            var s = state;
            if (!ModifiersCurrentlyApply(state, __instance)) return;
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

            var modifiers = GetCardModifiers(__instance, state, c);


            var actions = __instance.GetActionsOverridden(state, c);
            if(ShouldStickyNote(__instance, state, actions, modifiers))
            {
                //
                // draw index card / sticky note fix for floppables
                //

                // TODO: tell kokoro to stop rendering card weird

                RenderingActionsOnStickyNote = true;
                __instance.MakeAllActionIcons(g, g.state);
                RenderingActionsOnStickyNote = false;
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

        public static bool ModifiersCurrentlyApply(State s, Card card)
        {
            if (s.route is not Combat c) { return false; }
            if (s == DB.fakeState) { return false; }
            if (c == DB.fakeCombat) { return false; }
            if (c.routeOverride != null && !c.eyeballPeek) { return false; }
            if (card.drawAnim != 1) { return false; }
            
            return true;
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
