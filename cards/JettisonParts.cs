using CobaltCoreModding.Definitions.ExternalItems;
using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class JettisonParts : ModifierCard
    {
        public override string Name()
        {
            return "Jettison Parts";
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
            var stickers = new List<Spr>() {
                (Spr)MainManifest.sprites["icon_sticker_no_action"].Id,
                (Spr)MainManifest.sprites["icon_sticker_exhaust"].Id,
                (Spr)MainManifest.sprites["icon_sticker_evade"].Id,
            };

            if (upgrade == Upgrade.A)
            {
                stickers.Add((Spr)MainManifest.sprites["icon_sticker_hermes"].Id);
            }
            if (upgrade == Upgrade.B)
            {
                stickers.Add((Spr)MainManifest.sprites["icon_sticker_missile"].Id);
            }

            ModifiedCardsRegistry.RegisterMod(
                this, 
                c, 
                actionsModification: (List<CardAction> cardActions) =>
                {
                    List<CardAction> actions = new()
                    {
                        new AStatus() {
                            status = Enum.Parse<Status>("evade"),
                            targetPlayer = true,
                            statusAmount = 2,
                            mode = AStatusMode.Add,
                        }
                    };

                    if (upgrade == Upgrade.A)
                    {
                        actions.Add(new AStatus()
                        {
                            status = Enum.Parse<Status>("hermes"),
                            targetPlayer = true,
                            statusAmount = 1,
                            mode = AStatusMode.Add,
                        });
                    }
                    if (upgrade == Upgrade.B)
                    {
                        actions.Add(new ASpawn()
                        {
                            thing = new Missile
                            {
                                yAnimation = 0.0,
                                missileType = MissileType.normal,
                                targetPlayer = false
                            }
                        });
                    }

                    return actions;
                },
                dataModification: (CardData data) =>
                {
                    // note: CardData is a struct, so there's no need to copy it, it's totally safe to directly modify it
                    data.exhaust = true;
                    return data;
                },
                stickers: stickers
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
                        flippable = true,
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
            }
        }

        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var icons = new List<Icon>() {
                new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                new Icon((Spr)MainManifest.sprites["icon_no_action"].Id, null, Colors.textMain),
                new Icon(Enum.Parse<Spr>("icons_evade"), 1, Colors.textMain),
                new Icon(Enum.Parse<Spr>("icons_exhaust"), null, Colors.textMain),
            };

            var str = "have their effects replaced by \"gain 2 evade\" and gain exhaust.";

            if (upgrade == Upgrade.A)
            {
                icons.Add(new Icon(Enum.Parse<Spr>("icons_hermes"), 1, Colors.textMain));
                str = "have their effects replaced by \"gain 2 evade and 1 hermes boots\", and gain exhaust.";
            }
            if (upgrade == Upgrade.B)
            {
                icons.Add(new Icon(Enum.Parse<Spr>("icons_missile_normal"), 1, Colors.textMain));
                str = "have their effects replaced by \"gain 2 evade and launch a missile\", and gain exhaust.";
            }


            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"{GetTargetLocationString().Capitalize()} {str}"
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head),
                        // TODO: add glossary for evade
                        // TOOD: if upgrade a, hermes, if b, missile
                    },
                    icons = icons
                }
            };
        }
    }
}
