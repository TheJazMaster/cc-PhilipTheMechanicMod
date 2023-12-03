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
    [CardMeta(rarity = Rarity.rare, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class BlackMarketParts : ModifierCard
    {
        public override string Name()
        {
            return "Black Market Parts";
        }

        public override TargetLocation GetBaseTargetLocation() 
        {
            switch (upgrade)
            {
                default: return TargetLocation.SINGLE_LEFT;
                case Upgrade.A: return TargetLocation.SINGLE_LEFT;
                case Upgrade.B: return TargetLocation.SINGLE_RIGHT;
            }
        }

        public override void ApplyMod(Card c)
        {
            var stickers = new List<Spr>() {
                (Spr)MainManifest.sprites["icon_sticker_no_action"].Id,
                (Spr)MainManifest.sprites["icon_sticker_exhaust"].Id,
                (Spr)MainManifest.sprites["icon_sticker_add_card"].Id,
            };

            ModifiedCardsRegistry.RegisterMod(
                this, 
                c, 
                actionsModification: (List<CardAction> cardActions) =>
                {
                    List<CardAction> actions = new()
                    {
                        new AAddCard()
                        {
                            card = new UraniumRound() { upgrade = (this.upgrade == Upgrade.B ? Upgrade.B : Upgrade.None )},
                            destination = Enum.Parse<CardDestination>("Hand")
                        }
                    };

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
                        cost = 1,
                        unplayable = true,
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 1,
                        unplayable = true,
                        flippable = true,
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 1,
                        unplayable = true,
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
                new Icon(Enum.Parse<Spr>("icons_addCard"), null, Colors.textMain),
            };

            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"{GetTargetLocationString().Capitalize()} gains exhaust, removes all actions, and adds 1 Uranium Round{(upgrade == Upgrade.B ? " B" : "")} to your hand."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head),
                        // TODO: add glossary for no action
                        (new AAddCard() { card = new UraniumRound(){upgrade = (this.upgrade == Upgrade.B ? Upgrade.B : Upgrade.None )}, destination = Enum.Parse<CardDestination>("Hand") }).GetTooltips(s)[0]
                    }, 
                    icons = icons
                }
            };
        }
    }
}
