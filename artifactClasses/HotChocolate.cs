using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.artifacts
{
    [ArtifactMeta(pools = new[] { ArtifactPool.Common })]
    public class HotChocolate : Artifact
    {
        public override int? GetDisplayNumber(State s)
        {
            if (s.route is Combat c) {
                return c.hand.Where(c => c is ModifierCard && c.GetDataWithOverrides(s).unplayable).Count();
            }

            return null;
        }
    }
}
