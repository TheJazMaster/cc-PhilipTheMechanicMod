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
        public override void OnTurnStart(State state, Combat combat)
        {
            var redrawAmount = state.ship.Get((Status)MainManifest.statuses["redraw"].Id);

            if (redrawAmount <= 0) return;

            state.ship.Set((Status)MainManifest.statuses["redraw"].Id, redrawAmount - 1);
            combat.QueueImmediate(new ADrawCard() { count = 1 });
            Pulse();
        }
    }
}
