using clay.PhilipTheMechanic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    internal class ANanobots : CardAction
    {
        public required int thisCardUuid;

        public override void Begin(G g, State s, Combat c)
        {
            foreach (Card card in c.hand)
            {
                if (card.uuid == thisCardUuid) continue;
                if (card is Nanobots)
                {
                    s.RemoveCardFromWhereverItIs(card.uuid);
                    c.SendCardToExhaust(s, card);
                }
            }
        }
    }
}
