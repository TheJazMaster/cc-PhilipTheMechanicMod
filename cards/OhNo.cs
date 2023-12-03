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
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.B }, dontOffer = true)]
    public class OhNo : ModifierCard
    {
        public override string Name()
        {
            return "Oh No";
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
                    return new()
                    {
                        new AStatus() {
                            status = (Status)MainManifest.statuses["redraw"].Id,
                            targetPlayer = true,
                            statusAmount = upgrade == Upgrade.B ? 2 : 1,
                            mode = AStatusMode.Add,
                        }
                    };
                },
                dataModification: (CardData data) =>
                {
                    // note: CardData is a struct, so there's no need to copy it, it's totally safe to directly modify it
                    data.exhaust = true;
                    return data;
                },
                stickers: new() 
                { 
                    (Spr)MainManifest.sprites["icon_sticker_redraw"].Id, 
                    (Spr)MainManifest.sprites["icon_sticker_no_action"].Id, 
                    (Spr)MainManifest.sprites["icon_sticker_exhaust"].Id 
                }
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
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        retain = true,
                        //description = $"Increases the damage of every attack on {GetTargetLocationString()} by 1."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        retain = true,
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
                            text = $"{GetTargetLocationString().Capitalize()} do no actions, gain exhaust, and provide {(upgrade == Upgrade.B ? "2" : "1")} redraw."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head),
                        new TTGlossary(MainManifest.glossary["SRedraw"].Head),
                        new TTGlossary(MainManifest.glossary["ANoAction"].Head),
                    },
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_redraw"].Id, null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_no_action"].Id, null, Colors.textMain),
                    }
                }
            };
        }
    }
}
