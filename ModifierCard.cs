using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    public class ModifierCard : Card
    {
        public enum TargetLocation
        {
            SINGLE_LEFT,
            SINGLE_RIGHT,
            ALL_LEFT,
            ALL_RIGHT,
        }

        private List<Card> currentlyModifiedCards = new();

        // Explicitly has no actions
        public override List<CardAction> GetActions(State s, Combat c) { return new List<CardAction>(); }

        public override void OnDiscard(State s, Combat c)
        {
            foreach (Card card in currentlyModifiedCards) { ModifiedCardsRegistry.DeregisterMods(this, card); }
        }

        public override void OnDraw(State s, Combat c)
        {
            ModifyTargetCards(c.hand);
        }

        public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition)
        {
            foreach (Card card in currentlyModifiedCards) { ModifiedCardsRegistry.DeregisterMods(this, card); }
            ModifyTargetCards(c.hand);
        }

        private void ModifyTargetCards(List<Card> hand)
        {
            List<Card> leftCards = new List<Card>();
            List<Card> rightCards = new List<Card>();
            bool hasFoundSelf = false;
            foreach (Card card in hand)
            {
                if (card.uuid == this.uuid)
                {
                    hasFoundSelf = true;
                    continue;
                }

                if (hasFoundSelf) rightCards.Add(card);
                else              leftCards.Add(card);
            }

            var targetLocation = ApplyFlip(GetTargetLocation());

            switch(targetLocation)
            {
                case TargetLocation.SINGLE_LEFT:
                    {
                        var card = leftCards.LastOrDefault();
                        if (card == null) break;

                        this.ApplyMod(card);
                        currentlyModifiedCards = new() { card };
                        break;
                    }
                case TargetLocation.SINGLE_RIGHT:
                    {
                        var card = rightCards.FirstOrDefault();
                        if (card == null) break;

                        this.ApplyMod(card);
                        currentlyModifiedCards = new() { card };
                        break;
                    }
                case TargetLocation.ALL_LEFT:
                    {
                        foreach (Card card in leftCards) { this.ApplyMod(card); }
                        currentlyModifiedCards = leftCards;
                        break;
                    }
                case TargetLocation.ALL_RIGHT:
                    {
                        foreach (Card card in rightCards) { this.ApplyMod(card); }
                        currentlyModifiedCards = rightCards;
                        break;
                    }
                default:
                    throw new Exception("Unknown target location " + targetLocation);
            }
        }

        public TargetLocation ApplyFlip(TargetLocation targetLocation)
        {
            if (!base.flipped) return targetLocation;

            switch(targetLocation)
            {
                case TargetLocation.SINGLE_LEFT: return TargetLocation.SINGLE_RIGHT;
                case TargetLocation.SINGLE_RIGHT: return TargetLocation.SINGLE_LEFT;
                case TargetLocation.ALL_LEFT: return TargetLocation.ALL_RIGHT;
                case TargetLocation.ALL_RIGHT: return TargetLocation.ALL_LEFT;
            }

            throw new Exception("Unknown target location " + targetLocation);
        }

        public string GetTargetLocationString()
        {
            var targetLocation = ApplyFlip(GetTargetLocation());

            switch (targetLocation)
            {
                case TargetLocation.SINGLE_LEFT: return "the card to the left";
                case TargetLocation.SINGLE_RIGHT: return "the card to the right";
                case TargetLocation.ALL_LEFT: return "every card to the left";
                case TargetLocation.ALL_RIGHT: return "every card to the right";
            }

            throw new Exception("Unknown target location " + targetLocation);
        }

        public virtual void ApplyMod(Card c) { }
        public virtual TargetLocation GetTargetLocation() { return TargetLocation.SINGLE_LEFT; }
    }
}
