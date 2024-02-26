using clay.PhilipTheMechanic.Cards;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class EndlessToolbox : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Boss];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_endless_toolbox"].Sprite;
    public int counter = 2;

    public override int? GetDisplayNumber(State s)
    {
        return counter;
    }

    public override void OnTurnStart(State state, Combat combat)
    {
        counter = 2;
    }

    public override void OnCombatEnd(State state)
    {
        counter = 2;
    }
}

public class EndlessToolboxHook : IOnRedrawHook
{
    public void OnRedraw(Card card, State state, Combat combat)
    {
        var ownedEndlessToolbox = state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(EndlessToolbox)).FirstOrDefault() as EndlessToolbox;
        bool activateToolbox = ownedEndlessToolbox != null && ownedEndlessToolbox.counter > 0;
        if (activateToolbox)
        {
            ownedEndlessToolbox!.counter--;
            ownedEndlessToolbox!.Pulse();

            combat.QueueImmediate(new ADrawCard());
        }
    }
}