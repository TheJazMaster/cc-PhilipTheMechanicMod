using clay.PhilipTheMechanic.Cards;
using HarmonyLib;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace clay.PhilipTheMechanic.Artifacts;

[HarmonyPatch]
internal sealed class WireClippers : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_wire_clippers"].Sprite;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
    public static void HarmonyPostfix_Card_GetData(Card __instance, ref CardData __result, State state)
    {
        var owned = state.EnumerateAllArtifacts().Any((Artifact a) => a.GetType() == typeof(WireClippers));
        if (!owned) { return; }

        __result.unplayable = false;
        if (__instance.GetMeta().deck == Enum.Parse<Deck>("trash")) __result.exhaust = true;
    }
}
