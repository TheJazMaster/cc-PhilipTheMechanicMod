using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class PrecisionMachining : Card
    {
        public override string Name()
        {
            return "Precision Machining";
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            int index = c.hand.IndexOf(this);
            int redrawAmount = 1;

            if (c.hand.Count % 2 == 1 && index == c.hand.Count/2)
            {
                redrawAmount = upgrade == Upgrade.B ? 7 : 5;
            }

            return new()
            {
                new AStatus() {
                    status = (Status)MainManifest.statuses["redraw"].Id,
                    targetPlayer = true,
                    statusAmount = redrawAmount,
                    mode = Enum.Parse<AStatusMode>("Add"),
                }
            };
        }

        public override CardData GetData(State state) => new() { 
            cost = upgrade == Upgrade.A ? 1 : 2,
            description = $"If this card is in the center of your hand, add {(upgrade == Upgrade.B ? 7 : 5)} redraw, otherwise, add 1."
        };
    }
}
