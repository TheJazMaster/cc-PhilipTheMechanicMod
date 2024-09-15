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
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_endless_toolbox"];
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

public class EndlessToolboxHook : IRedrawStatusHook
{
    public void AfterRedraw(State state, Combat combat, Card card, IRedrawStatusHook possibilityHook, IRedrawStatusHook paymentHook, IRedrawStatusHook actionHook) {
        if (state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(EndlessToolbox)).FirstOrDefault() is not EndlessToolbox ownedEndlessToolbox) return;
        if (ownedEndlessToolbox.counter > 0) {
            ownedEndlessToolbox!.counter--;
            ownedEndlessToolbox!.Pulse();

            combat.QueueImmediate(new ADrawCard {
                count = 1
            });
        }
    }
}