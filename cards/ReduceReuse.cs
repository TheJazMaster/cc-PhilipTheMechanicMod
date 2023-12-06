using CobaltCoreModding.Definitions.ExternalItems;
using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhilipTheMechanic.ModifiedCardsRegistry;
using static System.Collections.Specialized.BitVector32;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class ReduceReuse : ModifierCard
    {
        public override string Name()
        {
            return "Reduce, Reuse";
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
                ModifierPriority.STANDARD,
                dataModification: (CardData data) =>
                {
                    // note: CardData is a struct, so there's no need to copy it, it's totally safe to directly modify it
                    data.recycle = true;
                    return data;
                },
                stickers: new() { (Spr)MainManifest.sprites["icon_sticker_recycle"].Id }
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
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 1,
                        flippable = true,
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 1,
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
                            text = $"Adds recycle to {GetTargetLocationString()}. Draw 1 on play."
                        },
                        new TTGlossary(GetGlossaryForTargetLocation().Head),
                        new TTGlossary("cardtrait.recycle")
                    },
                    icons = new() {
                        new Icon((Spr)GetIconSpriteForTargetLocation().Id, null, Colors.textMain),
                        new Icon(Enum.Parse<Spr>("icons_recycle"), null, Colors.textMain)
                    }
                },

                new ADrawCard()
                {
                    count = 1
                }
            };
        }
    }
}
