using Microsoft.Extensions.Logging;
using PhilipTheMechanic.actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhilipTheMechanic.ModifiedCardsRegistry;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class SpareParts : Card
    {
        public override string Name()
        {
            return "Spare Parts";
        }

        public override CardData GetData(State state)
        {
            return new()
            {
                cost = 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new()
            {
                new ADrawCard()
                {
                    count = upgrade == Upgrade.A ? 3 : 2,
                },
                new AStatus() {
                    status = (Status)MainManifest.statuses["redraw"].Id,
                    targetPlayer = true,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    mode = Enum.Parse<AStatusMode>("Add"),
                },
            };
        }
    }
}
