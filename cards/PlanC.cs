using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class PlanC : Card
    {
        public override string Name()
        {
            return "No Stock Parts";
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new()
            {
                new AAddCard()
                {
                    card = new OhNo() { upgrade = (this.upgrade == Upgrade.B ? Upgrade.B : Upgrade.None )},
                    destination = Enum.Parse<CardDestination>("Hand")
                }
            };
        }

        public override CardData GetData(State state) => new() { 
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = true
        };
    }
}
