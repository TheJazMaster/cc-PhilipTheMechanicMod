using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class NoStockParts : Card
    {
        public override string Name()
        {
            return "No Stock Parts";
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new()
            {
                new AStatus() {
                    status = (Status)MainManifest.statuses["customParts"].Id,
                    targetPlayer = true,
                    statusAmount = 1,
                    mode = Enum.Parse<AStatusMode>("Add"),
                },
            };
        }

        public override CardData GetData(State state) => new() { 
            cost = upgrade == Upgrade.A ? 2 : 3,
            exhaust = upgrade == Upgrade.B ? false : true
        };
    }
}
