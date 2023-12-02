using Microsoft.Extensions.Logging;
using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhilipTheMechanic.ModifierCard;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class ImpromptuBlastShield : ModifierCard
    {
        public override string Name()
        {
            return "Impromptu Blast Shield";
        }

        public override TargetLocation GetBaseTargetLocation()
        {
            return TargetLocation.ALL_RIGHT;
        }

        public override void ApplyMod(Card c)
        {
            ModifiedCardsRegistry.RegisterMod(
                this,
                c,
                actionsModification: (List<CardAction> cardActions) =>
                {
                    List<CardAction> overridenCardActions = new(cardActions);
                    
                    if (upgrade == Upgrade.B)
                        overridenCardActions.Add(new AStatus { targetPlayer = false, status = Enum.Parse<Status>("shield"), statusAmount = 1, mode = Enum.Parse<AStatusMode>("Add") });
                    else
                        overridenCardActions.Add(new AStatus { targetPlayer = false, status = Enum.Parse<Status>("tempShield"), statusAmount = 1, mode = Enum.Parse<AStatusMode>("Add") });

                    return overridenCardActions;
                },
                stickers: upgrade == Upgrade.B
                    ? new() { (Spr)MainManifest.sprites["icon_sticker_shield"].Id }
                    : new() { (Spr)MainManifest.sprites["icon_sticker_temp_shield"].Id }
            );
        }

        public override CardData GetData(State state)
        {
            switch (upgrade)
            {
                default:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        retain = true,
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        retain = true,
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = false,
                        retain = true,
                    };
            }
        }

        // TODO: this
        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            string icon = "icons_tempShield";
            string shieldType = "temp shield";

            if (upgrade == Upgrade.B)
            {
                icon = "icons_shield";
                shieldType = "shield";
            }

            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"{GetTargetLocationString().Capitalize()} applies 1 {shieldType}."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head)
                    },
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.heal),
                        new Icon(Enum.Parse<Spr>(icon), 1, Colors.textMain),
                    }
                }
            };
        }
    }
}
