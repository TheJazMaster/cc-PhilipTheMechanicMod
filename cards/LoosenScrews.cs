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

        public override TargetLocation GetTargetLocation()
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
                (List<CardAction> cardActions) =>
                {
                    List<CardAction> overridenCardActions = new(cardActions);
                    overridenCardActions.Add(new AHurt() { hurtAmount = 1, hurtShieldsFirst = false, targetPlayer = true });
                    return overridenCardActions;
                },
                (int energy) => Math.Max(0, energy-1)
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
                        description = $"{GetTargetLocationString().Capitalize()} costs 1 less energy but deals 1 hull damage."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 1,
                        unplayable = false,
                        flippable = true,
                        description = $"{GetTargetLocationString().Capitalize()} costs 1 less energy but deals 1 hull damage."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 1,
                        unplayable = false,
                        description = $"{GetTargetLocationString().Capitalize()} costs 1 less energy but deals 1 hull damage."
                    };
            }
        }
    }
}
