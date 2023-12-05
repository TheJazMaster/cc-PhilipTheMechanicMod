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
    [CardMeta(rarity = Rarity.common, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class OpenBayDoors : ModifierCard
    {
        public override string Name()
        {
            return "Open Bay Doors";
        }

        public override TargetLocation GetBaseTargetLocation()
        {
            switch (upgrade)
            {
                default: return TargetLocation.ALL_LEFT;
                case Upgrade.A: return TargetLocation.ALL_RIGHT;
                case Upgrade.B: return TargetLocation.ALL_LEFT;
            }
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
                        overridenCardActions.Add(new ASpawn() {
                            thing = new Missile
                            {
                                yAnimation = 0.0,
                                missileType = MissileType.normal,
                                targetPlayer = false
                            }
                        }); 

                    return overridenCardActions;
                },
                dataModification: (CardData data) =>
                {
                    // note: CardData is a struct, so there's no need to copy it, it's totally safe to directly modify it
                    data.exhaust = true;
                    data.unplayable = false;
                    return data;
                },
                stickers: upgrade == Upgrade.B
                    ? new() { (Spr)MainManifest.sprites["icon_sticker_exhaust"].Id, (Spr)MainManifest.sprites["icon_sticker_missile"].Id }
                    : new() { (Spr)MainManifest.sprites["icon_sticker_exhaust"].Id }
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
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 0,
                        unplayable = false,
                        retain = true
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                    };
            }
        }

        // TODO: this
        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var upgradeText = ".";
            List<Icon> icons = new() {
                new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.heal),
                new Icon(Enum.Parse<Spr>("icons_exhaust"), null, Colors.textMain),
            };

            if (upgrade == Upgrade.B)
            {
                upgradeText = " and launch a missile.";
                icons.Add(new Icon(Enum.Parse<Spr>("icons_missile_normal"), null, Colors.textMain));
            }

            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"{GetTargetLocationString().Capitalize()} gain exhaust{upgradeText} Makes unplayable cards playable."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head),
                        // TODO: add glossary for missile
                    },
                    icons = icons
                }
            };
        }
    }
}
