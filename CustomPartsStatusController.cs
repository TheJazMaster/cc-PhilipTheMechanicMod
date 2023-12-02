using FSPRO;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    [HarmonyPatch(typeof(Ship))]
    public class CustomPartsStatusController
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_Ship_OnBeginTurn(Ship __instance, State s, Combat c)
        {
            if (__instance.Get((Status)MainManifest.statuses["customParts"].Id) > 0)
            {
                c.QueueImmediate(new AStatus
                {
                    status = (Status)MainManifest.statuses["redraw"].Id,
                    statusAmount = __instance.Get((Status)MainManifest.statuses["customParts"].Id),
                    targetPlayer = __instance.isPlayerShip
                });
            }
        }
    }
}
