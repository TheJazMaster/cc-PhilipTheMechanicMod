using System;
using System.Linq;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class HotChocolate : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_hot_chocolate"];
}

// public class HotChocolateHook : IRedrawStatusHook
// {
//     static int recursionLevel = 0;
//     private static bool ConditionMet(State state, Combat combat) {
//         recursionLevel++;
//         if (recursionLevel >= 2) return combat.hand.Where(c => c.GetData(state).unplayable).Count() > 3;
//         else return combat.hand.Where(c => c.GetDataWithOverrides(state).unplayable).Count() > 3;
//     }
//     public bool? CanRedraw(State state, Combat combat, Card card)
//     {
// 		if (state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(HotChocolate)).FirstOrDefault() is not HotChocolate ownedHotChocolate) return null;

//         CardData data;
//         recursionLevel++;
//         if (recursionLevel >= 2) data = card.GetData(state);
//         else data = card.GetDataWithOverrides(state);
//         recursionLevel = 0;

// 		if (!data.unplayable) return null;

//         if (!ConditionMet(state, combat)) return null;

//         return true;
//     }

//     public bool PayForRedraw(State state, Combat combat, Card card, IRedrawStatusHook possibilityHook) {
//         return possibilityHook is HotChocolateHook;
//     }
// }