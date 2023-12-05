using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    [HarmonyPatch(typeof(State))]
    public static class StatePatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(State.RemoveAllTempCards))]
        public static bool HarmonyPostfix_Combat_RemoveAllTempCards(State __instance)
        {
            foreach (Card card in __instance.deck)
            {
                if (!card.GetDataWithOverrides(__instance).temporary) continue;
                if (card is ModifierCard mc2) { mc2.RemoveModifications(); }
            }

            return true;
        }
    }
}
