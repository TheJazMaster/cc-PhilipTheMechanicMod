using clay.PhilipTheMechanic.Cards;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class ScrapMagnet : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Boss];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_electromagnet"].Sprite;
    public int counter = 2;

    public override int? GetDisplayNumber(State s)
    {
        return counter;
    }

    public override void OnCombatEnd(State state)
    {
        counter = 2;
    }
}

public class ScrapMagnetHook : IAllowRedrawHook, IRedrawCostHook
{
    public bool AllowRedraw(Card card, State state, Combat combat)
    {
        bool hasRedraw = state.ship.Get(ModEntry.Instance.RedrawStatus.Status) > 0;
        if (hasRedraw) return false; // don't waste this artifact's effect if you still have redraw to spend

        var ownedScrapMagnet = state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(ScrapMagnet)).FirstOrDefault() as ScrapMagnet;
        if (ownedScrapMagnet == null) return false;
        if (ownedScrapMagnet.counter <= 0) return false;

        return combat.hand.IndexOf(card) == 0; 
    }

    public int RedrawCost(int currentCost, Card card, State state, Combat combat)
    {
        var allowRedraw = this.AllowRedraw(card, state, combat);
        if (allowRedraw)
        {
            var ownedScrapMagnet = state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(ScrapMagnet)).FirstOrDefault() as ScrapMagnet;
            ownedScrapMagnet!.counter--;

            return 0;
        }
        return currentCost;
    }
}