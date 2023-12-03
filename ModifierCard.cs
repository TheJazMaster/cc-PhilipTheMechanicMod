using CobaltCoreModding.Definitions.ExternalItems;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhilipTheMechanic.ModifierCard;

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
            NEIGHBORS,
            ALL
        }

        private List<Card> currentlyModifiedCards = new();

        // Explicitly has no actions
        public override List<CardAction> GetActions(State s, Combat c) { return new List<CardAction>(); }

        public override void OnDiscard(State s, Combat c)
        {
            foreach (Card card in currentlyModifiedCards) { ModifiedCardsRegistry.DeregisterMods(this, card); }
        }

        public override void OnExitCombat(State s, Combat c)
        {
            foreach (Card card in currentlyModifiedCards) { ModifiedCardsRegistry.DeregisterMods(this, card); }
        }

        public override void OnDraw(State s, Combat c)
        {
            MainManifest.Instance.Logger.LogInformation($"Drew a modifier card! {uuid}:{GetFullDisplayName()}");
            ModifyTargetCards(c.hand);
        }

        public override void OnFlip(G g)
        {
            if (g.state.route is Combat c)
            {
                foreach (Card card in currentlyModifiedCards) { ModifiedCardsRegistry.DeregisterMods(this, card); }
                ModifyTargetCards(c.hand);
            }
        }

        public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition)
        {
            MainManifest.Instance.Logger.LogInformation($"Modifier card {uuid}:{GetFullDisplayName()} acknowledges with due respect that another card was played.");

            foreach (Card card in currentlyModifiedCards) { ModifiedCardsRegistry.DeregisterMods(this, card); }
            ModifyTargetCards(c.hand);
        }

        public void OnOtherCardDiscardedWhileThisWasInHand(State s, Combat c)
        {
            MainManifest.Instance.Logger.LogInformation($"Modifier card {uuid}:{GetFullDisplayName()} acknowledges with due respect that another card was discarded.");

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

            var targetLocation = GetTargetLocation();

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
                case TargetLocation.NEIGHBORS:
                    {
                        var cards = (new List<Card>() { leftCards.LastOrDefault(), rightCards.FirstOrDefault() } ).WhereNotNull().ToList();
                        foreach (Card card in cards) { this.ApplyMod(card); }
                        currentlyModifiedCards = cards;
                        break;
                    }
                case TargetLocation.ALL:
                    {
                        foreach (Card card in hand) { this.ApplyMod(card); }
                        currentlyModifiedCards = new(hand);
                        break;
                    }

                default:
                    throw new Exception("Unknown target location " + targetLocation);
            }
        }

        public TargetLocation GetTargetLocation()
        {
            TargetLocation targetLocation = GetBaseTargetLocation();
            if (!base.flipped) return targetLocation;

            switch(targetLocation)
            {
                case TargetLocation.SINGLE_LEFT: return TargetLocation.SINGLE_RIGHT;
                case TargetLocation.SINGLE_RIGHT: return TargetLocation.SINGLE_LEFT;
                case TargetLocation.ALL_LEFT: return TargetLocation.ALL_RIGHT;
                case TargetLocation.ALL_RIGHT: return TargetLocation.ALL_LEFT;
                case TargetLocation.NEIGHBORS: return TargetLocation.NEIGHBORS;
                case TargetLocation.ALL: return TargetLocation.ALL;
            }

            throw new Exception("Unknown target location " + targetLocation);
        }

        public string GetTargetLocationString(bool anyInsteadOfEvery = false)
        {
            var targetLocation = GetTargetLocation();
            var anyEvery = anyInsteadOfEvery ? "any" : "every";

            switch (targetLocation)
            {
                case TargetLocation.SINGLE_LEFT: return "the card to the left";
                case TargetLocation.SINGLE_RIGHT: return "the card to the right";
                case TargetLocation.ALL_LEFT: return $"${anyEvery} card to the left";
                case TargetLocation.ALL_RIGHT: return $"${anyEvery} card to the right";
                case TargetLocation.NEIGHBORS: return "the card to the left and the card to the right";
                case TargetLocation.ALL: return "every card in your hand";
            }

            throw new Exception("Unknown target location " + targetLocation);
        }

        public ExternalGlossary GetGlossaryForTargetLocation()
        {
            var targetLocation = GetTargetLocation();
            switch (targetLocation)
            {
                case TargetLocation.SINGLE_LEFT: return MainManifest.glossary["ACardToTheLeft"];
                case TargetLocation.SINGLE_RIGHT: return MainManifest.glossary["ACardToTheRight"];
                case TargetLocation.ALL_LEFT: return MainManifest.glossary["AAllCardsToTheLeft"];
                case TargetLocation.ALL_RIGHT: return MainManifest.glossary["AAllCardsToTheRight"];
            }

            throw new Exception("Unknown target location " + targetLocation);
        }
        public ExternalSprite GetIconSpriteForTargetLocation()
        {
            var targetLocation = GetTargetLocation();
            switch (targetLocation)
            {
                case TargetLocation.SINGLE_LEFT: return MainManifest.sprites["icon_card_to_the_left"];
                case TargetLocation.SINGLE_RIGHT: return MainManifest.sprites["icon_card_to_the_right"];
                case TargetLocation.ALL_LEFT: return MainManifest.sprites["icon_all_cards_to_the_left"];
                case TargetLocation.ALL_RIGHT: return MainManifest.sprites["icon_all_cards_to_the_right"];
            }

            throw new Exception("Unknown target location " + targetLocation);
        }

        public virtual void ApplyMod(Card c) { }
        public virtual TargetLocation GetBaseTargetLocation() { return TargetLocation.SINGLE_LEFT; }
    }
}
