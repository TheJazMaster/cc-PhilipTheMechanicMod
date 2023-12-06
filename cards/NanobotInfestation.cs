using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhilipTheMechanic.ModifiedCardsRegistry;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class NanobotInfestation : ModifierCard
    {
        public override string Name()
        {
            return "Nanobot Infestation";
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
                ModifierPriority.LAST,
                actionsModification: (List<CardAction> cardActions, State s) =>
                {
                    List<CardAction> overridenCardActions = new(cardActions);
                    overridenCardActions.Add(new AReplay() { card = c });
                    overridenCardActions.Add(new AAddCardNoIcon() { card = new Nanobots(), destination = Enum.Parse<CardDestination>("Discard") });

                    overridenCardActions.Insert(0, new ADummyAction() { });
                    overridenCardActions.Insert(0, new ADummyAction() { });
                    return overridenCardActions;
                },
                dataModification: (CardData data) =>
                {
                    // TODO: this doesn't seem to work....

                    // note: CardData is a struct, so there's no need to copy it, it's totally safe to directly modify it
                    data.exhaust = true;
                    return data;
                },
                energyModification: (int energy) => {
                    return 0;
                },
                stickers: new List<Spr>() { (Spr)MainManifest.sprites["icon_sticker_exhaust"].Id, (Spr)MainManifest.sprites["icon_2x_sticker"].Id, (Spr)MainManifest.sprites["icon_sticker_add_card"].Id }
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
            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText()
                        {
                            text = $"{GetTargetLocationString(true).Capitalize()} plays twice for zero energy, gains exhaust, and adds 1 Nanobots to your discard pile."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head, null),
                        new TTGlossary(MainManifest.glossary["AReplay"].Head, null),
                        (new AAddCard() { card = new Nanobots(), destination = Enum.Parse<CardDestination>("Discard") }).GetTooltips(s)[0],
                        new TTCard() { card = new Nanobots() } 
                    },
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon((Spr)MainManifest.sprites["icon_play_twice"].Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_exhaust"), null, Colors.textMain),
                    }
                },

                new ATooltipDummy() {
                    tooltips = new() {},
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_addCard"), 1, Colors.textMain)
                    }
                }
            };
        }
    }
}
