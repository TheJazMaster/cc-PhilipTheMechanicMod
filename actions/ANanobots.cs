using clay.PhilipTheMechanic.Cards;
using Nickel;
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
            var cards = c.hand.ToList();
            foreach (Card card in cards)
            {
                if (card.uuid == thisCardUuid) continue;
                if (card is Nanobots)
                {
                    s.RemoveCardFromWhereverItIs(card.uuid);
                    c.SendCardToExhaust(s, card);
                }
            }
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(ModEntry.Instance.sprites["icon_nanobots"], null, Colors.redd);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            return [
                new GlossaryTooltip($"action.{GetType().Namespace!}::{GetType().Name}") {
                    TitleColor = Colors.action,
                    Icon = ModEntry.Instance.sprites["icon_nanobots"],
                    Title = ModEntry.Instance.Localizations.Localize(["action", "ANanobots", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["action", "ANanobots", "description"]),
                }
            ];
        }
    }
}
