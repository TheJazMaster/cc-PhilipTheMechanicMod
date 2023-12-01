using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class RecycleParts : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new()
            {
                new AStatus() {
                    status = (Status)MainManifest.statuses["redraw"].Id,
                    targetPlayer = true,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    mode = AStatusMode.Add,
                }
            };
        }

        public override CardData GetData(State state) => new() { cost = upgrade == Upgrade.A ? 0 : 1 };
    }
}
