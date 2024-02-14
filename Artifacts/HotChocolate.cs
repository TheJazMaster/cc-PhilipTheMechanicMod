using clay.PhilipTheMechanic.Cards;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class HotChocolate : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_hot_chocolate"].Sprite;
    public override int? GetDisplayNumber(State s)
    {
        if (s.route is Combat c)
        {
            return c.hand.Where(c => c.GetDataWithOverrides(s).unplayable).Count();
        }

        return null;
    }
}

public class HotChocolateHook : IAllowRedrawHook, IRedrawCostHook
{
    public bool AllowRedraw(Card card, State state, Combat combat)
    {
        var ownedHotChocolate = state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(HotChocolate)).FirstOrDefault() as HotChocolate;
        if (ownedHotChocolate == null) return false;

        var cardIsUnplayable = card.GetDataWithOverrides(state).unplayable;
        if (cardIsUnplayable) return false;

        int unplayableModCardCount = ownedHotChocolate == null ? 0 : combat.hand.Where(c => c.GetDataWithOverrides(state).unplayable).Count();
        if (unplayableModCardCount >= 3) return false;

        return true;
    }

    public int RedrawCost(int currentCost, Card card, State state, Combat combat)
    {
        if (this.AllowRedraw(card, state, combat)) return 0;
        return currentCost;
    }
}