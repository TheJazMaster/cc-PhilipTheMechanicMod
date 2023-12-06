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
            int index = c.hand.IndexOf(this);
            int redrawAmount = upgrade == Upgrade.A ? 2 : 1;

            if (c.hand.Count % 2 == 1 && index == c.hand.Count/2)
            {
                redrawAmount = upgrade == Upgrade.B ? 5 : 3;
            }

            return new()
            {
                new AStatusNoIcon() {
                    status = (Status)MainManifest.statuses["redraw"].Id,
                    targetPlayer = true,
                    statusAmount = redrawAmount,
                    mode = Enum.Parse<AStatusMode>("Add"),
                },
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"If this card is in the center of your hand, add {(upgrade == Upgrade.B ? 7 : 5)} redraw, otherwise, add 1."
                        },
                        new TTGlossary(MainManifest.glossary["CCardCentered"].Head),
                        new TTGlossary(MainManifest.glossary["CCardNotCentered"].Head),
                    },
                    icons = new()
                    {
                        new Icon((Spr)MainManifest.sprites["icon_card_is_centered"].Id, null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_redraw"].Id, upgrade == Upgrade.B ? 7 : 5, Colors.textMain)
                    }
                },
                new ATooltipDummy() {
                    tooltips = new() {},
                    icons = new()
                    {
                        new Icon((Spr)MainManifest.sprites["icon_card_is_not_centered"].Id, null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_redraw"].Id, upgrade == Upgrade.A ? 2 : 1, Colors.textMain)
                    }
                },
                new ADummyAction()
            };
        }

        public override CardData GetData(State state) => new() { 
            cost = 1,
        };
    }
}
