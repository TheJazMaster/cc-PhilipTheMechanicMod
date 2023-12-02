using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class DisableSafties : Card
    {
        public override string Name()
        {
            return "Disable Safties";
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new()
            {
                new AAddCard() { card = new Fear(), destination = Enum.Parse<CardDestination>("Hand") },
                new AAddCard() { card = new OverheatedCannons(), destination = Enum.Parse<CardDestination>("Hand") }
            };
        }

        public override CardData GetData(State state) => new() { cost = upgrade == Upgrade.A ? 0 : 1 };
    }
}
