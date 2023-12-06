using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    // CODE MODIFIED FROM
    // https://github.com/Shockah/Cobalt-Core-Mods/blob/dev/soggins/Soggins/FrogproofManager.cs#L54-L136
    [HarmonyPatch(typeof(Card))]
    public static class CardTraitManager
    {
        public class ExternalCardTrait
        {
            public Spr sprite;
            public string name;
            public Tooltip tooltip;

            public delegate bool CardHasExternalTrait(Card card);
            public CardHasExternalTrait testFunction;
        }
        private static List<ExternalCardTrait> externalCardTraits = new List<ExternalCardTrait>();

        public static void RegisterExternalCardTrait(ExternalCardTrait trait) { externalCardTraits.Add(trait); }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Card.Render))]
        private static IEnumerable<CodeInstruction> Card_Render_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
        {
            try
            {
                return new SequenceBlockMatcher<CodeInstruction>(instructions)
                    .AsGuidAnchorable()
                    .Find(
                        ILMatches.Ldloc<CardData>(originalMethod.GetMethodBody()!.LocalVariables),
                        ILMatches.Ldfld("buoyant"),
                        ILMatches.Brfalse
                    )
                    .PointerMatcher(SequenceMatcherRelativeElement.First)
                    .ExtractLabels(out var labels)
                    .AnchorPointer(out Guid findAnchor)
                    .Find(
                        ILMatches.Ldloc<Vec>(originalMethod.GetMethodBody()!.LocalVariables),
                        ILMatches.Ldfld("y"),
                        ILMatches.LdcI4(8),
                        ILMatches.Ldloc<int>(originalMethod.GetMethodBody()!.LocalVariables),
                        ILMatches.Instruction(OpCodes.Dup),
                        ILMatches.LdcI4(1),
                        ILMatches.Instruction(OpCodes.Add),
                        ILMatches.Stloc<int>(originalMethod.GetMethodBody()!.LocalVariables)
                    )
                    .PointerMatcher(SequenceMatcherRelativeElement.First)
                    .CreateLdlocInstruction(out var ldlocVec)
                    .Advance(3)
                    .CreateLdlocaInstruction(out var ldlocaCardTraitIndex)
                    .PointerMatcher(findAnchor)
                    .Insert(
                        SequenceMatcherPastBoundsDirection.Before, SequenceMatcherInsertionResultingBounds.IncludingInsertion,
                        new CodeInstruction(OpCodes.Ldarg_1).WithLabels(labels),
                        new CodeInstruction(OpCodes.Ldarg_3),
                        new CodeInstruction(OpCodes.Ldarg_0),
                        ldlocaCardTraitIndex,
                        ldlocVec,
                        new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(CardTraitManager), nameof(Card_Render_Transpiler_RenderTraitsIfNeeded)))
                    )
                    .AllElements();
            }
            catch (Exception ex)
            {
                MainManifest.Instance.Logger!.LogCritical("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, MainManifest.Instance.Name, ex);
                return instructions;
            }
        }

        private static void Card_Render_Transpiler_RenderTraitsIfNeeded(G g, State? state, Card card, ref int cardTraitIndex, Vec vec)
        {
            state ??= g.state;
            foreach (ExternalCardTrait trait in externalCardTraits)
            {
                if (!trait.testFunction(card)) continue;
                Draw.Sprite(trait.sprite, vec.x, vec.y - 8 * cardTraitIndex++);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetAllTooltips))]
        private static void HarmonyPostfix_Card_GetAllTooltips(Card __instance, G g, State s, bool showCardTraits, ref IEnumerable<Tooltip> __result)
        {
            if (!showCardTraits)
                return;
            //if (!Instance.FrogproofManager.IsFrogproof(s, s.route as Combat, __instance, FrogproofHookContext.Rendering))
            //    return;

            static IEnumerable<Tooltip> ModifyTooltips(IEnumerable<Tooltip> tooltips, Card __instance)
            {
                bool yieldedCustom = false;

                foreach (var tooltip in tooltips)
                {
                    if (!yieldedCustom && tooltip is TTGlossary glossary && glossary.key.StartsWith("cardtrait.") && glossary.key != "cardtrait.unplayable")
                    {
                        foreach (ExternalCardTrait trait in externalCardTraits)
                        {
                            if (!trait.testFunction(__instance)) continue;
                            yield return trait.tooltip;
                        }
                        yieldedCustom = true;
                    }
                    yield return tooltip;
                }

                if (!yieldedCustom)
                {
                    foreach (ExternalCardTrait trait in externalCardTraits)
                    {
                        if (!trait.testFunction(__instance)) continue;
                        yield return trait.tooltip;
                    }
                }
            }

            __result = ModifyTooltips(__result, __instance);
        }
    }
}