using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class FrenzyMod : ModifierCard
    {
        public override string Name()
        {
            return "Frenzy Mod";
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
                    overridenCardActions.Add(new AAttack() { damage = 1 });
                    if (upgrade == Upgrade.A) { overridenCardActions.Add(new AAttack() { damage = 1 }); }
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
                        cost = 0,
                        unplayable = true,
                        description = $"Add an additional 1 damage attack to {GetTargetLocationString()}."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        description = $"Add two additional 1 damage attacks to {GetTargetLocationString()}."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = true,
                        description = $"Add an additional 1 damage attack to {GetTargetLocationString()}."
                    };
            }
        }
    }
}
