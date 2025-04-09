using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace clay.PhilipTheMechanic.Actions;

[HarmonyPatch(typeof(Card))]
public abstract class AMultiIconAction : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [];
    }

    public override Icon? GetIcon(State s)
    {
        return null;
    }

    public virtual List<CardAction> GetActionsForRendering(State s) => [];


    static readonly int SPACING = 2;
    static int startingW = 0;
    static bool overrideIconWidth = false;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Card.RenderAction))]
    public static bool HarmonyPrefix_Card_RenderAction(ref int __result, G g, State state, CardAction action, bool dontDraw = false, int shardAvailable = 0, int stunChargeAvailable = 0, int bubbleJuiceAvailable = 0)
    {
        if (action is AMultiIconAction multiIconAction)
        {
            overrideIconWidth = true;
            var actions = multiIconAction.GetActionsForRendering(state);

            int width = -SPACING;
            if (dontDraw) {
                foreach(CardAction cardAction in actions) {
                    width += Card.RenderAction(g, state, cardAction, true, shardAvailable, stunChargeAvailable, bubbleJuiceAvailable) + SPACING;
                }
                __result = width;
                startingW = 0; overrideIconWidth = false;
                return false;
            }
            
            startingW = __result;
            foreach(CardAction cardAction in actions) {
                startingW = Card.RenderAction(g, state, cardAction, dontDraw, shardAvailable, stunChargeAvailable, bubbleJuiceAvailable) + SPACING;
            }
            startingW = 0; overrideIconWidth = false;

            return false;
        } 
        return true;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(nameof(Card.RenderAction))]
    public static IEnumerable<CodeInstruction> AdjustRenderingOffset(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
    {
        Label label = il.DefineLabel();
        return new SequenceBlockMatcher<CodeInstruction>(instructions).Find(SequenceBlockMatcherFindOccurence.First, SequenceMatcherRelativeBounds.WholeSequence,
            ILMatches.Stloc<Icon?>(originalMethod).CreateLdlocInstruction(out var ldLoc)
        ).Find(SequenceBlockMatcherFindOccurence.First, SequenceMatcherRelativeBounds.WholeSequence,
            ILMatches.AnyLdloca,
            ILMatches.LdcI4(0).Anchor(out var anchor),
            ILMatches.Stfld("w")
        )
        .Anchors()
        .PointerMatcher(anchor)
        .Replace(new List<CodeInstruction> {
            new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(AMultiIconAction), nameof(GetStartingWidth)))
        })
        .Find(SequenceBlockMatcherFindOccurence.First, SequenceMatcherRelativeBounds.WholeSequence,
            ILMatches.AnyLdloca,
            ILMatches.LdcI4(8).Anchor(out var anchor2),
            ILMatches.Stfld("iconWidth")
        )
        .Anchors()
        .PointerMatcher(anchor2)
        .Replace(new List<CodeInstruction> {
            ldLoc.Value,
            new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(AMultiIconAction), nameof(GetIconWidth)))
        })
        .AllElements();
    }

    private static int GetStartingWidth() {
        return startingW;
    }

    private static int GetIconWidth(Icon? icon) {
        if (overrideIconWidth && icon.HasValue && SpriteLoader.Get(icon.Value.path) is Texture2D texture) {
            return texture.Width - 1;
        }
        return 8;
    }
}