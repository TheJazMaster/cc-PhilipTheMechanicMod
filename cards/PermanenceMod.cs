using Microsoft.Extensions.Logging;
using PhilipTheMechanic.actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class PermanenceMod : ModifierCard
    {
        public override string Name()
        {
            return "Permanence Mod";
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
                actionsModification: (List<CardAction> cardActions, State s) =>
                {
                    List<CardAction> overridenCardActions = new();
                    int dmg = 0;
                    foreach (var action in cardActions)
                    {
                        if (action is AStatus status && status.targetPlayer && status.status == Enum.Parse<Status>("tempShield"))
                        {
                            if (upgrade == Upgrade.B)
                            {
                                var maxStatus = new AStatus();
                                maxStatus.status = Enum.Parse<Status>("maxShield");
                                maxStatus.statusAmount = status.statusAmount;
                                maxStatus.targetPlayer = true;
                                overridenCardActions.Add(maxStatus);
                            }

                            var newStatus = new AStatus();
                            newStatus.status = Enum.Parse<Status>("shield");
                            newStatus.statusAmount = status.statusAmount;
                            newStatus.targetPlayer = true;
                            overridenCardActions.Add(newStatus);

                        }
                        else
                        {
                            overridenCardActions.Add(action);
                        }
                    }

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
                    };
            }
        }

        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            //return new();

            var a = new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"{GetTargetLocationString().Capitalize()} applies shield instead of temp shield."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head)
                    },
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_x"), null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_equal"].Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_tempShield"), null, Colors.textMain),
                    }
                }
            };

            if (upgrade == Upgrade.B)
            {
                a.Add(
                    new ATooltipDummy()
                    {
                        tooltips = new(),
                        icons = new()
                        {
                            new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                            new Icon(Enum.Parse<Spr>("icons_maxShield"), null, Colors.textMain),
                            new Icon(Enum.Parse<Spr>("icons_x"), null, Colors.textMain),
                        }
                    }
                );
            }

            a.Add(
                new ATooltipDummy()
                {
                    tooltips = new(),
                    icons = new()
                    {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_shield"), null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_x"), null, Colors.textMain),
                    }
                }
            );

            return a;
        }
    }
}
