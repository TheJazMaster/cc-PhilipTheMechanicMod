using Microsoft.Extensions.Logging;
using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhilipTheMechanic.ModifiedCardsRegistry;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class ShieldingMod : ModifierCard
    {
        public override string Name()
        {
            return "Shielding Mod";
        }

        public override TargetLocation GetBaseTargetLocation()
        {
            switch (upgrade)
            {
                default: return TargetLocation.SINGLE_RIGHT;
                case Upgrade.A: return TargetLocation.ALL_LEFT;
                case Upgrade.B: return TargetLocation.SINGLE_RIGHT;
            }
        }

        public override void ApplyMod(Card c)
        {
            ModifiedCardsRegistry.RegisterMod(
                this,
                c,
                ModifierPriority.LATE, // same deal as Permanence Mod
                actionsModification: (List<CardAction> cardActions, State s) =>
                {
                    List<CardAction> overridenCardActions = new(cardActions);
                    int dmg = 0;
                    foreach (var action in cardActions)
                    {
                        if (action is AAttack attack)
                        {
                            dmg += attack.damage;
                        }
                    }

                    if (upgrade == Upgrade.B)
                        overridenCardActions.Add(new AStatus { targetPlayer = true, status = Enum.Parse<Status>("shield"), statusAmount = dmg, mode = Enum.Parse<AStatusMode>("Add") });
                    else
                        overridenCardActions.Add(new AStatus { targetPlayer = true, status = Enum.Parse<Status>("tempShield"), statusAmount = dmg, mode = Enum.Parse<AStatusMode>("Add") });

                    return overridenCardActions;
                },
                stickers: upgrade == Upgrade.B 
                    ? new() { (Spr)MainManifest.sprites["icon_sticker_shield_attack"].Id }
                    : new() { (Spr)MainManifest.sprites["icon_sticker_temp_shield_attack"].Id }
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
                        unplayable = true,
                        flippable = true,
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        flippable = true,
                    };
            }
        }

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
                            text = $"{GetTargetLocationString().Capitalize()} applies {shieldType} equal to its total damage."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head)
                    },
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_x"), null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_equal"].Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_attack"), null, Colors.textMain),
                    }
                },
                new ATooltipDummy()
                {
                    icons = new()
                    {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>(icon), null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_x"), null, Colors.textMain),
                    }
                }
            };
        }
    }
}
