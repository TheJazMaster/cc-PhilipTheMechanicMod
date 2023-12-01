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
                default: return TargetLocation.SINGLE_LEFT;
                case Upgrade.A: return TargetLocation.SINGLE_LEFT;
                case Upgrade.B: return TargetLocation.SINGLE_LEFT;
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
                    overridenCardActions.Add(new AHurt() { hurtAmount = (upgrade == Upgrade.B ? 2 : 1), hurtShieldsFirst = false, targetPlayer = true });
                    return overridenCardActions;
                },
                energyModification: (int energy) => {
                    if (upgrade == Upgrade.B) return 0;
                    return Math.Max(0, energy - 1);
                },
                stickers: upgrade == Upgrade.B
                    ? new() { (Spr)MainManifest.sprites["icon_sticker_0_energy"].Id, (Spr)MainManifest.sprites["icon_sticker_hull_damage"].Id, (Spr)MainManifest.sprites["icon_sticker_hull_damage"].Id }
                    : new() { (Spr)MainManifest.sprites["icon_sticker_energy_discount"].Id, (Spr)MainManifest.sprites["icon_sticker_hull_damage"].Id }
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
                        //description = $"{GetTargetLocationString().Capitalize()} costs 1 less energy but deals 1 hull damage."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 1,
                        unplayable = false,
                        flippable = true,
                        //description = $"{GetTargetLocationString().Capitalize()} costs 1 less energy but deals 1 hull damage."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 1,
                        unplayable = false,
                        flippable = true,
                        //description = $"{GetTargetLocationString().Capitalize()} costs 0 energy but deals 2 hull damage."
                    };
            }
        }

        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            string desc;

            if (upgrade == Upgrade.B)
            {
                desc = $"{GetTargetLocationString().Capitalize()} costs 0 energy but deals 2 hull damage.";
            }
            else
            {
                desc = $"{GetTargetLocationString().Capitalize()} costs 1 less energy but deals 1 hull damage.";
            }

            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText() { text = desc },
                        new TTGlossary(GetGlossaryForTargetLocation().Head, null),
                        new TTGlossary("action.hurt", (upgrade == Upgrade.B ? 2 : 1)),
                        upgrade == Upgrade.B 
                            ? MainManifest.vanillaSpritesGlossary["ASetEnergy"] 
                            : MainManifest.vanillaSpritesGlossary["AEnergyDiscount"],
                    },
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
