using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.artifacts
{
    // all unplayable cards become playable
    [ArtifactMeta(pools = new[] { ArtifactPool.Common })]
    public class SelfPropellingCannons : Artifact
    {
        public override void OnCombatStart(State state, Combat combat)
        {
            State state2 = state;
            combat.QueueImmediate(from pair in state2.ship.parts.Select((Part part, int x) => new { part, x })
                                  where pair.part.type == PType.cannon
                                  select new ABrittle
                                  {
                                      targetPlayer = true,
                                      worldX = state2.ship.x + pair.x,
                                      //justTheActiveOverride = true,
                                      artifactPulse = Key()
                                  });
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
}
