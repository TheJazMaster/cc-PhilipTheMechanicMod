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
            public CardActionsModification? actionsModification;
            public CardEnergyModification? energyModification;
        }

        public delegate List<CardAction> CardActionsModification(List<CardAction> cardActions);
        public delegate int CardEnergyModification(int originalEnergy);
        public static Dictionary<int, List<CardModRegistration>> cardMods = new();

        public static void RegisterMod(Card self, Card modifiedCard, CardActionsModification? actionsModification, CardEnergyModification? energyModification)
        {
            MainManifest.Instance.Logger.LogInformation($"Card modification being registered for {modifiedCard.uuid}:`{modifiedCard.GetFullDisplayName()}` by {self.uuid}:`{self.GetFullDisplayName()}`");

            if (!cardMods.ContainsKey(modifiedCard.uuid)) { cardMods[modifiedCard.uuid] = new List<CardModRegistration>(); }
            cardMods[modifiedCard.uuid].Add(new CardModRegistration() { 
                from = self, 
                actionsModification = actionsModification,
                energyModification = energyModification
            });
        }

        public static void DeregisterMods(Card moddingCard, Card moddedCard)
        {
            if (!cardMods.ContainsKey(moddedCard.uuid)) { return; }
            MainManifest.Instance.Logger.LogInformation($"Deregistering card modification for {moddedCard.uuid}:`{moddedCard.GetFullDisplayName()}` by {moddingCard.uuid}:`{moddingCard.GetFullDisplayName()}`");

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
            if (c.routeOverride != null && !c.eyeballPeek) { return; }
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }

            List<CardAction> overridenCardActions = __result;
            foreach (var registration in cardMods[__instance.uuid])
            {
                if (registration.actionsModification == null) continue;
                overridenCardActions = registration.actionsModification(overridenCardActions);
            }

            __result = overridenCardActions;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.GetCurrentCost))]
        public static void HarmonyPostfix_Card_GetCurrentCost(Card __instance, ref int __result, State s)
        {
            if (s.route is Combat c && c.routeOverride != null && !c.eyeballPeek) { return; }
            if (!cardMods.ContainsKey(__instance.uuid)) { return; }

            int cost = __result;
            foreach (var registration in cardMods[__instance.uuid])
            {
                if (registration.energyModification == null) continue;

                MainManifest.Instance?.Logger?.LogInformation($"Applying energy modification for {__instance.uuid}:`{__instance.GetFullDisplayName()}` from {registration.from.uuid}:`{registration.from.GetFullDisplayName()}`");
                cost = registration.energyModification(cost);
            }

            __result = cost;
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
