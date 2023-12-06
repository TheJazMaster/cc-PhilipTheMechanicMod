using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.rare, dontOffer = true, upgradesTo = new[] { Upgrade.B })]
    public class UraniumRound : Card
    {
        public override string Name()
        {
            return "Uranium Round";
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new()
            {
                new AAttack()
                {
                    damage = GetDmg(s, 1),
                    piercing = true,
                    stunEnemy = true,
                }
            };
        }

        public override CardData GetData(State state) => new() { cost = 0, temporary = true, exhaust = (upgrade != Upgrade.B) };
    }
}
