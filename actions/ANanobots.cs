using clay.PhilipTheMechanic.Cards;
using Shockah;
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
            return new Icon(ModEntry.Instance.sprites["icon_nanobots"].Sprite, null, Colors.redd);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            return [
                new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites["icon_nanobots"].Sprite,
                    () => ModEntry.Instance.Localizations.Localize(["action", "ANanobots", "name"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "ANanobots", "description"]),
                    key: typeof(ANanobots).FullName ?? typeof(ANanobots).Name
                )
            ];
        }
    }
}
