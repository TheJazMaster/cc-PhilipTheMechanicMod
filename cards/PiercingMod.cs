using CobaltCoreModding.Definitions.ExternalItems;
using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class PiercingMod : ModifierCard
    {
        public override string Name()
        {
            return "Piercing Mod";
        }

        public override TargetLocation GetBaseTargetLocation() 
        {
            switch (upgrade)
            {
                default: return TargetLocation.SINGLE_RIGHT;
                case Upgrade.A: return TargetLocation.SINGLE_RIGHT;
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
                    List<CardAction> overridenCardActions = new();
                    foreach (var action in cardActions)
                    {
                        if (action is AAttack attack)
                        {
                            var newAttack = Mutil.DeepCopy(attack);
                            newAttack.piercing = true;
                            overridenCardActions.Add(newAttack);
                        }
                        else
                        {
                            overridenCardActions.Add(action);
                        }
                    }
                    return overridenCardActions;
                },
                stickers: new() { (Spr)MainManifest.sprites["icon_sticker_piercing"].Id }
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
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        flippable = true,
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
            }
        }

        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"Makes every attack on {GetTargetLocationString()} piercing."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head),
                        new TTGlossary("action.attackPiercing")
                    },
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_attackPiercing"), null, Colors.textMain)
                    }
                }
            };
        }
    }
}
