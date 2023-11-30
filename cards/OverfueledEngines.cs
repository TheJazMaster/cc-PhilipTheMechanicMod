using PhilipTheMechanic.actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new[] { Upgrade.A, Upgrade.B })]
    public class OverfueledEngines : ModifierCard
    {
        public override string Name()
        {
            return "Overfueled Engines";
        }

        public override TargetLocation GetTargetLocation()
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
            ModifiedCardsRegistry.RegisterMod(
                this,
                c,
                (List<CardAction> cardActions) =>
                {
                    List<CardAction> overridenCardActions = new(cardActions);
                    overridenCardActions.Add(new AReplay() { card = c });

                    if (upgrade == Upgrade.A) overridenCardActions.Add(new AAddCard() { card = new TrashFumes(), destination = CardDestination.Deck });
                    else                      overridenCardActions.Add(new AAddCard() { card = new Toxic(),      destination = CardDestination.Hand });

                    return overridenCardActions;
                },
                null
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
                        description = $"{GetTargetLocationString().Capitalize()} plays twice, then adds a Toxic to your hand."
                    };
                case Upgrade.A:
                    return new()
                    {
                        cost = 0,
                        unplayable = false,
                        description = $"{GetTargetLocationString().Capitalize()} plays twice, then adds a Fumes to your deck."
                    };
                case Upgrade.B:
                    return new()
                    {
                        cost = 0,
                        unplayable = false,
                        flippable = true,
                        description = $"{GetTargetLocationString().Capitalize()} plays twice, then adds a Toxic to your hand."
                    };
            }
        }

        //public override IEnumerable<Tooltip> GetAllTooltips(G g, State s, bool showCardTraits = true)
        //{

        //}

        // NOTE: this is only here for the tooltip, this card isn't actually supposed to have any actions
        public override List<CardAction> GetActions(State s, Combat c) 
        {
            if (upgrade == Upgrade.A) return new List<CardAction>() { new ATooltipDummy() { tooltips = new() { new TTCard() { card = new TrashFumes() } } } };
            else                      return new List<CardAction>() { new ATooltipDummy() { tooltips = new() { new TTCard() { card = new Toxic()      } } } };
        }
    }
}
