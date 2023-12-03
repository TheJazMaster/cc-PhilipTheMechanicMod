using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
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

        public override void OnDiscard(State s, Combat c)
        {
            c.SendCardToDiscard(s, new Nanobots());
        }

        // TODO: must be temporary, also add the TTCard tooltip
        public override CardData GetData(State state) => new() { 
            unplayable = true,
            temporary = true,
            cost = 1,
            description = "On discard, add one additional Nanobots to your discard pile."
        };
    }
}
