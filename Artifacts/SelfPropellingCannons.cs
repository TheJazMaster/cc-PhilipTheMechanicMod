using clay.PhilipTheMechanic.Cards;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class SelfPropellingCannons : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_self_propelling_cannons"].Sprite;

    // should be OnCombatStart, but then GlassCannons overrides this effect
    public override void OnTurnStart(State state, Combat combat)
    {
        State state2 = state;
        combat.QueueImmediate(
            from pair in state2.ship.parts.Select((Part part, int x) => new { part, x })
            where pair.part.type == PType.cannon && pair.part.active == true
            select new ABrittle
            {
                targetPlayer = true,
                worldX = state2.ship.x + pair.x,
                //justTheActiveOverride = true,
                artifactPulse = Key()
            }
        );
    }

    public override void OnTurnEnd(State state, Combat combat)
    {
        combat.QueueImmediate(
            from pair in state.ship.parts.Select((Part part, int x) => new { part, x })
            where pair.part.type == PType.cannon
            select new ASpawn()
            {
                fromX = pair.x,
                thing = new Missile
                {
                    yAnimation = 0.0,
                    missileType = MissileType.normal,
                    targetPlayer = false
                }
            }
        );

        Pulse();
    }
}
