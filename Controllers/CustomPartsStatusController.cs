using FSPRO;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Controllers
{
    [HarmonyPatch(typeof(Ship))]
    public class CustomPartsStatusController
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Ship.OnBeginTurn))]
        public static void HarmonyPostfix_Ship_OnBeginTurn(Ship __instance, State s, Combat c)
        {
            int amt = __instance.Get(ModEntry.Instance.CustomPartsStatus.Status);
            if (amt > 0)
            {
                c.QueueImmediate(new AStatus
                {
                    status = ModEntry.Instance.RedrawStatus,
                    statusAmount = amt,
                    targetPlayer = __instance.isPlayerShip
                });
            }
        }
    }
}
