using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.artifacts
{
    // all unplayable cards become playable
    [HarmonyPatch(typeof(Card))]
    [ArtifactMeta(pools = new[] { ArtifactPool.Common })]
    public class WireClippers : Artifact
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetDataWithOverrides))]
        public static void HarmonyPostfix_Card_GetData(Card __instance, ref CardData __result, State state)
        {
            var owned = state.EnumerateAllArtifacts().Any((Artifact a) => a.GetType() == typeof(WireClippers));
            if (!owned) { return; }

            __result.unplayable = false;
            if (__instance.GetMeta().deck == Enum.Parse<Deck>("trash"))  __result.exhaust = true;
        }
    }
}
