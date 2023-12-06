using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class LoosenScrews : ModifierCard
    {
        public override string Name()
        {
            return "Loosen Screws";
        }

        public override TargetLocation GetBaseTargetLocation()
        {
            switch (upgrade)
            {
                default: return TargetLocation.SINGLE_RIGHT;
                case Upgrade.A: return TargetLocation.SINGLE_RIGHT;
                case Upgrade.B: return TargetLocation.SINGLE_RIGHT;
            }
        }

        public override void ApplyMod(Card c)
        {
            ModifiedCardsRegistry.RegisterMod(
                this, 
                c, 
                actionsModification: (List<CardAction> cardActions, State s) =>
                {
                    List<CardAction> overridenCardActions = new(cardActions);
                    overridenCardActions.Add(
                        new AStatus()
                        {
                            status = Enum.Parse<Status>("energyLessNextTurn"),
                            targetPlayer = true,
                            statusAmount = upgrade == Upgrade.B ? 2 : 1,
                            mode = Enum.Parse<AStatusMode>("Add"),
                        }
                    );
                    return overridenCardActions;
                },
                energyModification: (int energy) => {
                    if (upgrade == Upgrade.B) return 0;
                    return Math.Max(0, energy - 1);
                },
                stickers: upgrade == Upgrade.B
                    ? new() { (Spr)MainManifest.sprites["icon_sticker_0_energy"].Id, (Spr)MainManifest.sprites["icon_sticker_energyLessNextTurn"].Id }
                    : new() { (Spr)MainManifest.sprites["icon_sticker_energy_discount"].Id, (Spr)MainManifest.sprites["icon_sticker_energyLessNextTurn"].Id }
            );
        }

        public override CardData GetData(State state)
        {
            switch (upgrade)
            {
                default:
                    return new()
                    {
                        cost = 1,
                        unplayable = true,
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 1,
                        unplayable = false,
                        flippable = true,
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 1,
                        unplayable = false,
                        flippable = true,
                    };
            }
        }

        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            string desc;

            if (upgrade == Upgrade.B)
            {
                desc = $"{GetTargetLocationString().Capitalize()} costs 0 energy but removes 2 energy next turn.";
            }
            else
            {
                desc = $"{GetTargetLocationString().Capitalize()} costs 1 less energy but removes 1 energy next turn.";
            }

            List<Tooltip> tooltips = new() {
                new TTText() { text = desc },
                new TTGlossary(GetGlossaryForTargetLocation().Head, null),
                upgrade == Upgrade.B
                    ? MainManifest.vanillaSpritesGlossary["ASetEnergy"]
                    : MainManifest.vanillaSpritesGlossary["AEnergyDiscount"],
            };

            tooltips.AddRange(
                new AStatus()
                {
                    status = Enum.Parse<Status>("energyLessNextTurn"),
                    targetPlayer = true,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    mode = Enum.Parse<AStatusMode>("Add"),
                }.GetTooltips(s)
            );

            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = tooltips,
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.heal),
                        new Icon(Enum.Parse<Spr>("icons_hurt"), (upgrade == Upgrade.B ? 2 : 1), Colors.hurt),
                        upgrade == Upgrade.B 
                            ? new Icon(Enum.Parse<Spr>("icons_energy"), 0, Colors.energy)
                            : new Icon(Enum.Parse<Spr>("icons_discount"), 1, Colors.energy),

                    }
                }
            };
        }
    }
}
