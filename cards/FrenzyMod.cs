using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class FrenzyMod : ModifierCard
    {
        public override string Name()
        {
            return "Frenzy Mod";
        }

        public override TargetLocation GetBaseTargetLocation()
        {
            switch (upgrade)
            {
                default: return TargetLocation.SINGLE_LEFT;
                case Upgrade.A: return TargetLocation.SINGLE_LEFT;
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
                    overridenCardActions.Add(new AAttack() { damage = 1 });
                    if (upgrade == Upgrade.A) { overridenCardActions.Add(new AAttack() { damage = 1 }); }
                    return overridenCardActions;
                },
                stickers: upgrade == Upgrade.A 
                    ? new() { (Spr)MainManifest.sprites["icon_sticker_attack"].Id, (Spr)MainManifest.sprites["icon_sticker_attack"].Id }
                    : new() { (Spr)MainManifest.sprites["icon_sticker_attack"].Id }
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
                        //description = $"Add an additional 1 damage attack to {GetTargetLocationString()}."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        flippable = true,
                        //description = $"Add two additional 1 damage attacks to {GetTargetLocationString()}."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        flippable = true,
                        //description = $"Add an additional 1 damage attack to {GetTargetLocationString()}."
                    };
            }
        }
        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c)
        {
            string desc;

            switch (upgrade)
            {
                default:        desc = $"Add an additional 1 damage attack to {GetTargetLocationString()}."; break;
                case Upgrade.A: desc = $"Add two additional 1 damage attacks to {GetTargetLocationString()}."; break;
                case Upgrade.B: desc = $"Add an additional 1 damage attack to {GetTargetLocationString()}."; break;
            }

            List<Icon> icons = upgrade == Upgrade.A
                ? new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        //new Icon((Spr)MainManifest.sprites["icon_extra_attack"].Id, 1, Colors.heal),
                        //new Icon((Spr)MainManifest.sprites["icon_extra_attack"].Id, 1, Colors.heal)
                        new Icon(Enum.Parse<Spr>("icons_attack"), 1, Colors.hurt),
                        new Icon(Enum.Parse<Spr>("icons_attack"), 1, Colors.hurt)
                    }
                : new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        //new Icon((Spr)MainManifest.sprites["icon_extra_attack"].Id, 1, Colors.heal)
                        new Icon(Enum.Parse<Spr>("icons_attack"), 1, Colors.hurt)
                    };

            return new List<CardAction>() {
                new ATooltipDummy() {
                    tooltips = new() {
                        new TTText() { text = desc },
                        new TTGlossary(GetGlossaryForTargetLocation().Head),
                        //new TTGlossary(MainManifest.glossary["AExtraAttack"].Head),
                        //new TTGlossary("action.attack.name")
                    },
                    icons = icons
                }
            };
        }
    }
}
