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

    internal static readonly string FlimsyIdsKey = "FlimsyIds";

    private static List<(int, Upgrade)> cardCache = [];
    private static List<List<CardModifier>> _LastCachedModifiers = [[]];
    public static List<List<CardModifier>> LastCachedModifiers {
        get { return _LastCachedModifiers; }
        private set { _LastCachedModifiers = value; }
    }


    public static void InvalidateCache() {
        cardCache = [];
    }

    public static List<List<CardModifier>> CalculateCardModifiers(State s, Combat c)
    {
        if (cardCache.SequenceEqual(c.hand.Select(card => (card.uuid, card.upgrade)))) return LastCachedModifiers;

        List<List<AModifierWrapper>> modifierSources = Enumerable.Range(0, c.hand.Count).Select(l => new List<AModifierWrapper>()).ToList();
        List<List<CardModifier>> modificationList = Enumerable.Range(0, c.hand.Count).Select(l => new List<CardModifier>()).ToList();
        
        bool extendMods = false; bool saveFlimsy = false;
        int ind;
        int latestDeleteIndex = -1;
        for (ind = 0; ind < c.hand.Count; ind++) {
            Card card = c.hand[ind];
            int skip = 0;
            if (card is ExtenderMod emod) {
                extendMods |= emod.ExtendsMods();
                saveFlimsy |= emod.SaveFlimsy();
                if (latestDeleteIndex != -1) {
                    for (int j = 0; j < ind; j++) {
                        if (j != latestDeleteIndex) modifierSources[j] = [];
                        skip += c.hand.Count;
                    }
                }
            } else if (card is ModifierCard modifierCard) {
                foreach (AModifierWrapper wrapper in modifierCard.GetModifierActionsOverriden(s, c))
                {
                    modifierSources[ind].Add(wrapper);
                    foreach (CardModifier modifier in wrapper.modifiers) {
                        if (modifier is MDeleteActions) {
                            latestDeleteIndex = ind;
                            if (extendMods || wrapper is AWholeHandCardsModifierWrapper w) {
                                for (int j = 0; j < ind; j++) modifierSources[j] = [];
                                skip += c.hand.Count;
                            } else if (wrapper is ASingleDirectionalCardModifierWrapper sd) {
                                if (sd.left && ind > 0) modifierSources[ind-1] = [];
                                else if (!sd.left && ind < c.hand.Count - 1) modifierSources[ind+1] = [];
                            } else if (wrapper is AWholeHandDirectionalCardsModifierWrapper wh) {
                                if (wh.left) {
                                    for (int j = 0; j < ind; j++) modifierSources[j] = [];
                                }
                                else if (!wh.left && ind < c.hand.Count - 1) skip += c.hand.Count;
                            } else if (wrapper is ANeighboringCardsModifierWrapper) {
                                if (ind > 0) modifierSources[ind-1] = [];
                                skip++;
                            }
                        }
                    }
                }
            }
            ind += skip;
        }

        bool hasHotChocolate = s.EnumerateAllArtifacts().Any(artifact => artifact is HotChocolate);

        for (ind = 0; ind < c.hand.Count; ind++) {
            Card card = c.hand[ind];
            foreach (AModifierWrapper wrapper in modifierSources[ind])
            {
                for (int j = 0; j < c.hand.Count; j++) {
                    if ((extendMods && ind != j) || (wrapper.IsTargeting(card, ind, j, c, hasHotChocolate ? 2 : 1) && wrapper.modifiers.Count > 0)) {
                        modificationList[j].AddRange(wrapper.modifiers);
                        if (!saveFlimsy && wrapper.isFlimsy) {
                            modificationList[j].Last().flimsyUuids.Add(card.uuid);
                        }
                    }
                }
            }
        }

        for (ind = 0; ind < modificationList.Count; ind++) {
            modificationList[ind].Sort((mod1, mod2) => mod2.Priority.CompareTo(mod1.Priority));
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
        .Insert(SequenceMatcherPastBoundsDirection.Before, SequenceMatcherInsertionResultingBounds.IncludingInsertion, new List<CodeInstruction> {
            new CodeInstruction(OpCodes.Dup).WithLabels(extractedLabels),
            new(OpCodes.Isinst, typeof(ADirectionalCardModifierWrapper)),
            new(OpCodes.Brfalse, label),
            new(OpCodes.Ldc_I4_1),
            stLoc
        })
        .AllElements();
    }


    private static bool isDuringRender = false;
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
        int index = c.hand.IndexOf(__instance);
        if (index < 0 || index >= LastCachedModifiers.Count) return;

        CalculateCardModifiers(s, c);
        foreach (CardModifier modifier in LastCachedModifiers[index]) {
            if (modifier is ICardActionModifier ca) {
                __result = ca.TransformActions(__result, s, c, __instance, isDuringRender);
            }
        }

        if (!isDuringRender) {
            string dialogueSelector = $".{__instance.GetMeta().deck.Key()}Card_ModifiedBy_{__instance.GetMeta().deck.Key()}";

            __result.Add(ModEntry.Instance.KokoroApi.Actions.MakeHidden(new ADummyAction { dialogueSelector = dialogueSelector }));
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
    private static void ApplyDataModifiers(Card __instance, ref CardData __result, State state)
    {
        if (state.route is not Combat c || !ModifiersCurrentlyApply(state, c, __instance)) return;
        int index = c.hand.IndexOf(__instance);
        if (index < 0 || index >= LastCachedModifiers.Count) return;

        CalculateCardModifiers(state, c);
        foreach (CardModifier modifier in LastCachedModifiers[index]) {
            if (modifier is ICardDataModifier cd) {
                __result = cd.TransformData(__result, state, c, __instance, isDuringRender);
            }
        }
    }

    public static bool ModifiersCurrentlyApply(State s, Combat c, Card card)
    {
        return !(c == DB.fakeCombat || (isDuringRender && card.drawAnim != 1));
    }

    public static void HandleFlimsyModifiers(Card playedCard, State s, Combat c, int handPosition)
    {
        if (handPosition < 0 || handPosition >= LastCachedModifiers.Count) return;

        List<Card> cardsToDiscard = [];
        foreach (CardModifier modifier in LastCachedModifiers[handPosition]) {
            foreach (int uuid in modifier.flimsyUuids) {
                Card? foundCard = s.FindCard(uuid);
                if (foundCard != null) cardsToDiscard.Add(foundCard);
            }
        }

        foreach (Card card in cardsToDiscard)
        {
            if (c.hand.Contains(card)) {
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