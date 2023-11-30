using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    [HarmonyPatch(typeof(Card))]
    public static class ModifiedCardsRegistry
    {
        public struct CardModRegistration
        {
            public Card from;
            public CardModification mod;
        }

        public delegate List<CardAction> CardModification(List<CardAction> cardActions);
        public static Dictionary<int, List<CardModRegistration>> cardMods = new();

        public static void RegisterMod(Card self, Card modifiedCard, CardModification mod)
        {
            MainManifest.Instance.Logger.LogInformation($"Card modification being registered for {modifiedCard.uuid}:`{modifiedCard.GetFullDisplayName()}` by {self.uuid}:`{self.GetFullDisplayName()}`");

            if (!cardMods.ContainsKey(modifiedCard.uuid)) { cardMods[modifiedCard.uuid] = new List<CardModRegistration>(); }
            cardMods[modifiedCard.uuid].Add(new CardModRegistration() { from = self, mod = mod });
        }

        public static void DeregisterMods(Card moddingCard, Card moddedCard)
        {
            if (!cardMods.ContainsKey(moddedCard.uuid)) { return; }
            for (int i = 0; i < cardMods[moddedCard.uuid].Count; i++)
            {
                if (cardMods[moddedCard.uuid][i].from == moddingCard)
                {
                    cardMods[moddedCard.uuid].RemoveAt(i);
                    i--;
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetActionsOverridden))]
        public static void HarmonyPostfix_Card_GetActions(Card __instance, ref List<CardAction> __result, State s, Combat c)
        {
            if (!cardMods.ContainsKey(__instance.uuid)) {  return; }

            List<CardAction> overridenCardActions = __result;
            foreach(var action in __result)
            {
                foreach (var registration in cardMods[__instance.uuid])
                {
                    MainManifest.Instance?.Logger?.LogInformation($"Applying card modification for {__instance.uuid}:`{__instance.GetFullDisplayName()}` from {registration.from.uuid}:`{registration.from.GetFullDisplayName()}`");
                    overridenCardActions = registration.mod(overridenCardActions);
                }
            }

            __result = overridenCardActions;
        }

        //[HarmonyPostfix]
        //[HarmonyPatch(nameof(Card.GetFullDisplayName))]
        //public static void TEMP(Card __instance, ref string __result)
        //{
        //    if (!cardMods.ContainsKey(__instance.uuid)) { return; }

        //    __result = "MODIFIED " + __result;
        //}
    }
}
