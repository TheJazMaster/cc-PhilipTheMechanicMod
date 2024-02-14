using clay.PhilipTheMechanic.Cards;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class AngleGrinder : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_angle_grinder"].Sprite;

    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new AStatus
        {
            status = ModEntry.Instance.Api.RedrawStatus.Status,
            targetPlayer = true,
            statusAmount = 3,
            artifactPulse = Key()
        });
    }
}
