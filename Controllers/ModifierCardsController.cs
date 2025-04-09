using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using clay.PhilipTheMechanic.Artifacts;
using clay.PhilipTheMechanic.Cards;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace clay.PhilipTheMechanic.Controllers;

[HarmonyPatch]
public static class ModifierCardsController
{
    static IModHelper Helper => ModEntry.Instance.Helper;
    internal static bool SuppressActionMods = false;

    private static List<(int, Upgrade)> cardCache = [];

    private static List<List<CardModifier>> _LastCachedModifiers = [[]];
    public static List<List<CardModifier>> LastCachedModifiers {
        get { return _LastCachedModifiers; }
        private set { _LastCachedModifiers = value; }
    }

    private static Dictionary<int, Dictionary<int, bool>> _ModifierUsage = [];
    public static Dictionary<int, Dictionary<int, bool>> ModifierUsage {
        get { return _ModifierUsage; }
        private set { _ModifierUsage = value; }
    }


    public static void InvalidateCache() {
        cardCache.Clear();
        ModifierUsage.Clear();
    }

    public static List<List<CardModifier>> CalculateCardModifiers(State s, Combat c)
    {
        if (cardCache.SequenceEqual(c.hand.Select(card => (card.uuid, card.upgrade)))) return LastCachedModifiers;

        List<List<AModifierWrapper>> modifierSources = Enumerable.Range(0, c.hand.Count).Select(l => new List<AModifierWrapper>()).ToList();
        List<List<CardModifier>> modificationList = Enumerable.Range(0, c.hand.Count).Select(l => new List<CardModifier>()).ToList();
        ModifierUsage.Clear();
        for (int i = 0; i < c.hand.Count; i++) ModifierUsage[i] = [];
        
        int ind;
        // int latestDeleteIndex = -1;

        bool hasHotChocolate = s.EnumerateAllArtifacts().Any(artifact => artifact is HotChocolate);
        int range = hasHotChocolate ? 2 : 1;

        for (ind = 0; ind < c.hand.Count; ind++) {
            Card card = c.hand[ind];
            Deck deck = card.GetMeta().deck;

            if (StatusMeta.deckToMissingStatus.TryGetValue(card.GetMeta().deck, out var status) && s.ship.Get(status) > 0) continue;

            // int skip = 0;
            if (card is ModifierCard modifierCard) {
                foreach (AModifierWrapper wrapper in modifierCard.GetModifierActionsOverriden(s, c))
                {
                    ModifierCard.SetSource(wrapper, deck, card.uuid);
                    modifierSources[ind].Add(wrapper);
                    // Removes modifiers from overwritten cards
                //     if (wrapper.overwrites) {
                //         latestDeleteIndex = ind;
                //         if (extendMods || wrapper is AWholeHandCardsModifierWrapper w) {
                //             for (int j = 0; j < ind; j++) modifierSources[j] = [];
                //             skip += c.hand.Count;
                //         } else if (wrapper is ASingleDirectionalCardModifierWrapper sd) {
                //             if (sd.left && ind > 0) {
                //                 if (hasHotChocolate && ind-1 > 0) modifierSources[ind-2] = [];
                //                 modifierSources[ind-1] = [];
                //             }
                //             else if (!sd.left && ind < c.hand.Count - 1) {
                //                 if (hasHotChocolate) skip++;
                //                 skip++;
                //             }
                //         } else if (wrapper is AWholeHandDirectionalCardsModifierWrapper wh) {
                //             if (wh.left) {
                //                 for (int j = 0; j < ind; j++) modifierSources[j] = [];
                //             }
                //             else if (!wh.left && ind < c.hand.Count - 1) skip += c.hand.Count;
                //         } else if (wrapper is ANeighboringCardsModifierWrapper) {
                //             if (ind > 0) modifierSources[ind-1] = [];
                //             skip++;
                //             if (hasHotChocolate) {
                //                 if (ind-1 > 0) modifierSources[ind-2] = [];
                //                 skip++;
                //             }
                //         }
                //     }
                }
            }
            // ind += skip;
        }

        List<(int i, AModifierWrapper wrapper, CardModifier modifier)> flattened = [];
        for (ind = 0; ind < c.hand.Count; ind++) {
            flattened.AddRange(modifierSources[ind].SelectMany(item => item.GetCardModifiers().Where(m => m is IModifierModifier).Select<CardModifier, (int, AModifierWrapper, CardModifier)>(i => (ind, item, i))));
        }
        flattened = [.. flattened.OrderByDescending(item => item.modifier.Priority)];
        foreach ((int i, AModifierWrapper wrapper, CardModifier modifier) in flattened) {
            Card card = c.hand[i];
            for (int j = 0; j < c.hand.Count; j++) {
                if (wrapper.selector.IsTargeting(card, i, j, c, range)) {
                    (modifier as IModifierModifier)!.TransformModifiers(modifierSources[j], s, c, card, null);
                }
            }
        }
        for (ind = 0; ind < c.hand.Count; ind++) {
            Card card = c.hand[ind];
            int sourceUuid = card.uuid;
            foreach (AModifierWrapper wrapper in modifierSources[ind])
            {
                List<CardModifier> modifiers = wrapper.GetCardModifiers();
                modifiers.ForEach(mod => mod.sourceUuid = sourceUuid);
                if (wrapper.isFlimsy) modifiers.ForEach(mod => mod.fromFlimsy = mod.IgnoresFlimsy);

                for (int j = 0; j < c.hand.Count; j++) {
                    if (wrapper.selector.IsTargeting(card, ind, j, c, range) && modifiers.Count > 0) {
                        modificationList[j].AddRange(modifiers);
                        // if (wrapper.isFlimsy) {
                        //     modificationList[j].Last().flimsyUuids.Add(card.uuid);
                        // }
                    }
                }
            }
        }

        for (ind = 0; ind < modificationList.Count; ind++) {
            modificationList[ind].Sort((mod1, mod2) => mod2.Priority.CompareTo(mod1.Priority));
        }

        // Dummy modification to description cards to apply stickers
        for (ind = 0; ind < c.hand.Count; ind++) {
            Card card = c.hand[ind];
            if (!ModifierCardsRenderingController.IsDescriptionCard(card, s, c)) continue;

            foreach (CardModifier modifier in modificationList[ind]) {
                if (modifier is ICardActionModifier ca) {
                    ca.TransformActions(card.GetActions(s, c), s, c, card, true, out bool success);
                    if (success) MarkModified(ind, modifier.sourceUuid, modifier.fromFlimsy);
                }
            }
        }

        LastCachedModifiers = modificationList;
        cardCache = c.hand.Select(card => (card.uuid, card.upgrade)).ToList();
        return modificationList;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
    public static IEnumerable<CodeInstruction> MakeModifierCardsFlippable(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
    {
        Label label = il.DefineLabel();
        return new SequenceBlockMatcher<CodeInstruction>(instructions).Find(
            ILMatches.Instruction(OpCodes.Dup),
            ILMatches.Isinst<ADroneMove>(),
            ILMatches.Brfalse,
            ILMatches.LdcI4(1),
            ILMatches.Stloc<bool>(originalMethod).CreateStlocInstruction(out var stLoc)
        )
        .PointerMatcher(SequenceMatcherRelativeElement.Last)
        .Advance(1)
        .ExtractLabels(out var extractedLabels).AddLabel(label)
        .Insert(SequenceMatcherPastBoundsDirection.Before, SequenceMatcherInsertionResultingBounds.IncludingInsertion, [
            new CodeInstruction(OpCodes.Dup).WithLabels(extractedLabels),
            new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ModifierCardsController), nameof(IsFlippableMod))),
            new(OpCodes.Brfalse, label),
            new(OpCodes.Ldc_I4_1),
            stLoc
        ])
        .AllElements();
    }

    private static bool IsFlippableMod(CardAction action) {
        return action is AModifierWrapper wrapper && wrapper.selector is DirectionalSelector;
    }

    private static void MarkModified(int handIndex, int sourceUuid, bool flimsy) {
        if (handIndex < 0 || handIndex >= ModifierUsage.Count) return;

        var value = ModifierUsage[handIndex];
        value[sourceUuid] = flimsy || value.GetValueOrDefault(sourceUuid, false);
    }


    internal static bool isDuringRender = false;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Card), nameof(Card.Render))]
    public static void TrackRenderingPre(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
    {
        isDuringRender = true;
    }
    [HarmonyFinalizer]
    [HarmonyPatch(typeof(Card), nameof(Card.Render))]
    public static void TrackRenderingPost(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
    {
        isDuringRender = false;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
    public static void ApplyActionModifiers(Card __instance, ref List<CardAction> __result, State s, Combat c)
    {
        if (!ModifiersCurrentlyApply(s, c, __instance)) return;
        CalculateCardModifiers(s, c);

        int index = c.hand.IndexOf(__instance);
        if (index < 0 || index >= LastCachedModifiers.Count || LastCachedModifiers[index].Count == 0) return;

        List<AModifierWrapper>? wrappers = null;

        foreach (CardModifier modifier in LastCachedModifiers[index]) {
            if (modifier is IModifierModifier mo) {
                wrappers ??= __result.TakeWhile(a => a is AModifierWrapper).Cast<AModifierWrapper>().ToList();
                mo.TransformModifiers(wrappers, s, c, __instance, index, out bool success);
                if (success) MarkModified(index, modifier.sourceUuid, modifier.fromFlimsy);
            }
            if (modifier is ICardActionModifier ca) {
                __result = ca.TransformActions(__result, s, c, __instance, isDuringRender, out bool success);
                if (success) MarkModified(index, modifier.sourceUuid, modifier.fromFlimsy);
            }
        }

        if (!isDuringRender && LastCachedModifiers[index].Count > 0) {
            string dialogueSelector = $".{__instance.GetMeta().deck.Key()}Card_ModifiedBy_{LastCachedModifiers[index].Last().sourceDeck.Key()}";

            __result.Add(ModEntry.Instance.KokoroApi.Actions.MakeHidden(new ADummyAction { dialogueSelector = dialogueSelector }));
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
    private static void ApplyDataModifiers(Card __instance, ref CardData __result, State state)
    {
        if (MG.inst.g.metaRoute != null || state.route is not Combat c || !ModifiersCurrentlyApply(state, c, __instance)) return;

        CalculateCardModifiers(state, c);

        int index = c.hand.IndexOf(__instance);
        if (index < 0 || index >= LastCachedModifiers.Count) return;

        foreach (CardModifier modifier in LastCachedModifiers[index]) {
            if (modifier is ICardDataModifier cd) {
                __result = cd.TransformData(__result, state, c, __instance, isDuringRender, out bool success);
                if (success) MarkModified(index, modifier.sourceUuid, modifier.fromFlimsy);
            }
        }
    }

    public static bool ModifiersCurrentlyApply(State s, Combat c, Card card)
    {
        return !(s.routeOverride != null || c == DB.fakeCombat || (isDuringRender && card.drawAnim != 1));
    }

    public static void HandleFlimsyModifiers(Card playedCard, State s, Combat c, int handPosition)
    {
        if (handPosition < 0 || handPosition >= ModifierUsage.Count) return;

        // List<Card> cardsToDiscard = [];
        // foreach (CardModifier modifier in LastCachedModifiers[handPosition]) {
        //     foreach (int uuid in modifier.flimsyUuids) {
        //         Card? foundCard = s.FindCard(uuid);
        //         if (foundCard != null) cardsToDiscard.Add(foundCard);
        //     }
        // }

        // foreach (Card card in cardsToDiscard)
        // {
        //     if (c.hand.Contains(card)) {
        //         s.RemoveCardFromWhereverItIs(card.uuid);
        //         c.SendCardToDiscard(s, card);
        //     }
        // }
        foreach ((int uuid, bool flimsy) in ModifierUsage[handPosition])
        {
            if (!flimsy) continue;
            if (c.hand.First(card => card.uuid == uuid) is { } card) {
                s.RemoveCardFromWhereverItIs(card.uuid);
                c.SendCardToDiscard(s, card);
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Ship), nameof(Ship._Set))]
    private static void InvalidateCacheDueToStatus(Status status, int n) {
        InvalidateCache();
    }
}