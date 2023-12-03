using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class EmergencyTraining : Card
    {
        public override string Name()
        {
            return "Emergency Training";
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new()
            {
                new ADiscard(),
                new AStatus() {
                    status = (Status)MainManifest.statuses["redraw"].Id,
                    targetPlayer = true,
                    statusAmount = 5,
                    mode = AStatusMode.Add,
                },
                new AAddCard() { card = new ImpromptuBlastShield() { upgrade = (this.upgrade == Upgrade.B ? Upgrade.B : Upgrade.None) }, destination = Enum.Parse<CardDestination>("Hand") },
                new ADrawCard() { count=4 },
            };
        }

        public override CardData GetData(State state) => new() { 
            cost = upgrade == Upgrade.A ? 2 : 3,
            exhaust = true
        };
    }
}
