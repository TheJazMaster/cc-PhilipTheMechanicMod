using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.artifacts
{
    // gain 3 redraw at the start of each battle
    [ArtifactMeta(pools = new[] { ArtifactPool.Common })]
    public class AngleGrinder : Artifact
    {
        public override void OnCombatStart(State state, Combat combat)
        {
            combat.QueueImmediate(new AStatus
            {
                status = (Status)MainManifest.statuses["redraw"].Id,
                targetPlayer = true,
                statusAmount = 3,
                mode = Enum.Parse<AStatusMode>("Add"),
                artifactPulse = Key()
            });
        }
    }
}
