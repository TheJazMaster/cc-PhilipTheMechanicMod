using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            int uncenterdAmount = upgrade == Upgrade.A ? 2 : 1;
            int centeredAmount = upgrade == Upgrade.B ? 7 : 5;
            int redrawAmount = uncenterdAmount;

            int index = c.hand.IndexOf(this);
            bool centered = c.hand.Count % 2 == 1 && index == c.hand.Count / 2;

            if (centered)
            {
                redrawAmount = centeredAmount;
            }

            List<CardAction> actions = new()
            {
                new AStatusNoIcon() {
                    status = (Status)MainManifest.statuses["redraw"].Id,
                    targetPlayer = true,
                    statusAmount = redrawAmount,
                    mode = Enum.Parse<AStatusMode>("Add"),
                },
                new ADummyAction(),
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"If this card is in the center of your hand, add {centeredAmount} redraw and draw 2 cards, otherwise, add {uncenterdAmount} redraw."
                        },
                        new TTGlossary(MainManifest.glossary["CCardCentered"].Head),
                        new TTGlossary(MainManifest.glossary["CCardNotCentered"].Head),
                    },
                    icons = new()
                    {
                        new Icon((Spr)MainManifest.sprites["icon_card_is_centered" + (centered? "" : "_disabled")].Id, null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_redraw"].Id, centeredAmount, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_drawCard"), 2, Colors.textMain)
                    }
                },
                new ATooltipDummy() {
                    tooltips = new() {},
                    icons = new()
                    {
                        new Icon((Spr)MainManifest.sprites["icon_card_is_not_centered" + (centered? "_disabled" : "")].Id, null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_redraw"].Id, uncenterdAmount, Colors.textMain)
                    }
                },
                new ADummyAction(),
            };

            if (centered)
            {
                actions.Add(new ADrawNoIcon()
                {
                    count = 2
                });
            }
            else
            {
                actions.Add(new ADummyAction());
            }

            return actions;
        }

        public override CardData GetData(State state) => new() { 
            cost = 1,
        };
    }
}
