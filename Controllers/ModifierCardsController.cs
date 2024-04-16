using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace clay.PhilipTheMechanic.Controllers
{
    [HarmonyPatch]
    public static class ModifierCardsController
    {
        class Cache
        {
            static Dictionary<int, List<CardAction>> actionsCache = new();
            static Dictionary<int, List<ICardModifier>> modifiersCache = new();
            static double lastTime = -1;

            static void CleanCache(State s)
            {
                if (lastTime == s.time) return;
                lastTime = s.time;

                actionsCache.Clear();
                modifiersCache.Clear();
            }

            public static List<CardAction>? FetchActions(State s, Card c)
            {
                CleanCache(s);

                if (!actionsCache.ContainsKey(c.uuid)) return null;
                return actionsCache[c.uuid];
            }

            public static void StoreActions(Card c, List<CardAction> actions)
            {
                actionsCache[c.uuid] = actions;
            }

            public static List<ICardModifier>? FetchModifiers(State s, Card c)
            {
                CleanCache(s);

                if (!modifiersCache.ContainsKey(c.uuid)) return null;
                return modifiersCache[c.uuid];
            }

            public static void StoreModifiers(Card c, List<ICardModifier> modifiers)
            {
                modifiersCache[c.uuid] = modifiers;
            }
        }

        // patch get actions and stuff like that
        internal static bool SuppressActionMods = false;

        public sealed class Prioirites
        {
            public static readonly double REMOVE_ALL_ACTIONS = 999.0;

            public static readonly double MODIFY_DATA_UNFAVORABLE = 51.0;
            public static readonly double MODIFY_DATA_FAVORABLE = 50.0;

            public static readonly double MODIFY_ENERGY_FIRST = 50.0;
            public static readonly double MODIFY_ENERGY_LAST = 49.0;

            public static readonly double ADD_ACTION = 10;
            public static readonly double ADD_ACTION_LAST = 5;
            public static readonly double MODIFY_ALL_ACTIONS = 0;
        }

        public static List<ICardModifier> GetCardModifiers(Card target, State s, Combat c, bool recurse = true)
        {
            if (Cache.FetchModifiers(s, target) is List<ICardModifier> cached) return cached;
            
            List<ICardModifier> modifiers = new List<ICardModifier>(8);
            foreach (Card card in c.hand)
            {
                if (recurse)
                {
                    // check to see if `card` has its actions removed, if so, none of its modifiers should apply
                    if (GetCardModifiers(card, s, c, false).Any(m => m is MDeleteActions))
                    {
                        continue;
                    }
                }

                SuppressActionMods = true;
                var actions = Cache.FetchActions(s, card) ?? card.GetActionsOverridden(s, c);
                SuppressActionMods = false;

                foreach (CardAction action in actions)
                {
                    if (action is not AModifierWrapper wrapper) continue;
                    if (!wrapper.IsTargeting(target, card, c)) continue;

                    modifiers.AddRange(wrapper.modifiers);
                }
            }

            modifiers.Sort((mod1, mod2) => mod2.Priority.CompareTo(mod1.Priority));
            Cache.StoreModifiers(target, modifiers);
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

        private static bool isDuringRender = false;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Card), nameof(Card.Render))]
        public static void TrackRenderingPre(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            isDuringRender = true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.Render))]
        public static void TrackRenderingPost(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            isDuringRender = false;
        }



        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
        public static void ApplyActionModifiers(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (SuppressActionMods) return;
            if (s.route is Combat combat && combat.routeOverride != null && !combat.eyeballPeek) { return; }
            if (s.route is not Combat) { return; }

            // if the cache doesn't return null, just use what it has
            //if (Cache.FetchActions(s, __instance) is List<CardAction> actions) { __result = actions; return; }

            List<CardAction> overridenCardActions = __result;
            var modifiers = GetCardModifiers(__instance, s, c);
            foreach (ICardModifier modifier in modifiers)
            {
                overridenCardActions = modifier.TransformActions(overridenCardActions, s, c, __instance, isDuringRender);
            }

            try
            {
                string dialogueSelector = $".{ModEntry.Instance.Helper.Content.Decks.LookupByDeck(__instance.GetMeta().deck)!.UniqueName}Card_ModifiedByPhilip";
                overridenCardActions.Add(new ADummyAction() { dialogueSelector = dialogueSelector });
                overridenCardActions.Insert(0, new ADummyAction() { });
            }
            catch (Exception e) { }

            __result = overridenCardActions;

            //Cache.StoreActions(__instance, overridenCardActions);
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
                data = modifier.TransformData(data, state, c, __instance, isDuringRender);
            }

            __result = data;
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
