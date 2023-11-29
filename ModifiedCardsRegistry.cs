using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    public static class ModifiedCardsRegistry
    {
        public struct CardModRegistration
        {
            public Card from;
            public CardModification mod;
        }

        public delegate List<CardAction> CardModification(List<CardAction> cardActions);
        public static Dictionary<Card, List<CardModRegistration>> cardMods = new();

        public static void RegisterMod(Card self, Card modifiedCard, CardModification mod)
        {
            if (!cardMods.ContainsKey(modifiedCard)) { cardMods[modifiedCard] = new List<CardModRegistration>(); }
            cardMods[modifiedCard].Add(new CardModRegistration() { from = self, mod = mod });
        }

        public static void DeregisterMods(Card moddingCard, Card moddedCard)
        {
            if (!cardMods.ContainsKey(moddedCard)) { return; }
            for (int i = 0; i < cardMods[moddedCard].Count; i++)
            {
                if (cardMods[moddedCard][i].from == moddingCard)
                {
                    cardMods[moddedCard].RemoveAt(i);
                    i--;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Card), "GetActions")]
        public static void HarmonyPostfix_Card_GetActions(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (!cardMods.ContainsKey(__instance)) {  return; }

            List<CardAction> overridenCardActions = __result;
            foreach(var action in __result)
            {
                foreach (var registration in cardMods[__instance])
                {
                    overridenCardActions = registration.mod(overridenCardActions);
                }
            }

            __result = overridenCardActions;
        }
    }
}
