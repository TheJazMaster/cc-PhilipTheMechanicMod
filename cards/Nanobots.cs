using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [HarmonyPatch(typeof(State))]
    [CardMeta(deck = Deck.trash, rarity = Rarity.common, dontOffer = true)]
    public class Nanobots : Card
    {
        public override string Name()
        {
            return "Nanobots";
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new() {};
        }

        //public override void OnDiscard(State s, Combat c)
        //{
        //    c.SendCardToDiscard(s, new Nanobots());
        //}

        [HarmonyPostfix]
        [HarmonyPatch(nameof(State.ShuffleDeck))]
        public static void HarmonyPostfix_State_ShuffleDeck(State __instance, bool isMidCombat = false)
        {
            try
            {
                if (isMidCombat && __instance.route is Combat combat)
                {
                    int nanobotsCount = 0;
                    foreach (Card card in __instance.deck)
                    {
                        if (card is Nanobots n) { nanobotsCount++; }
                    }

                    for (int i = 0; i < nanobotsCount; i++)
                    {
                        __instance.SendCardToDeck(new Nanobots(), doAnimation: false, insertRandomly: true);
                    }

                    for (int i = 0; i < nanobotsCount; i++)
                    {
                        var cardIdx = __instance.rngShuffle.NextInt() % __instance.deck.Count;
                        var card = __instance.deck[cardIdx];
                        __instance.deck.RemoveAt(cardIdx);
                        combat.SendCardToExhaust(__instance, card);
                    }
                }
            }
            catch (Exception e)
            {
                MainManifest.Instance.Logger.LogError("Error in nanobot replication");
                MainManifest.Instance.Logger.LogError(e.ToString());
                MainManifest.Instance.Logger.LogError(e.StackTrace);
            }
        }

        // TODO: must be temporary, also add the TTCard tooltip
        public override CardData GetData(State state) => new() { 
            unplayable = true,
            temporary = true,
            cost = 1,
            exhaust = true,
            description = "On shuffle deck, exhaust random card and gain Nanobots."
        };
    }
}
