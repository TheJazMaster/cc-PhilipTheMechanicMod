using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.artifacts
{
    [ArtifactMeta(pools = new[] { ArtifactPool.Boss })]
    public class ScrapMagnet : Artifact
    {
        public int counter = 2;

        public override int? GetDisplayNumber(State s)
        {
            return counter;
        }

        public override void OnTurnStart(State state, Combat combat)
        {
        }

        public override void OnCombatEnd(State state)
        {
            counter = 2;
        }
    }
}
