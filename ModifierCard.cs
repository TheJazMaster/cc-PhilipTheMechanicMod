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



            public override void ExtraRender(G g, Vec v)
            {
                // just... man with effects that directly write to the state of the hand, what else can I do?
                // I suppose I should probably hook into something like Combat.DrainActions but this is much easier
                if (g.state.route is Combat c)
                {
                    ReapplyModifications(c);
                }
            }

        public override void OnDiscard(State s, Combat c)
        {
            RemoveModifications();
        }

        public override void OnExitCombat(State s, Combat c)
        {
            RemoveModifications();
        }

        public void RemoveModifications()
        {
            foreach (Card card in currentlyModifiedCards) { ModifiedCardsRegistry.DeregisterMods(this, card); }
        }

        public void ReapplyModifications(Combat c)
        {
            RemoveModifications();
            ModifyTargetCards(c.hand);
        }

        public override void OnDraw(State s, Combat c)
        {
            //MainManifest.Instance.Logger.LogInformation($"Drew a modifier card! {uuid}:{GetFullDisplayName()}");
            ModifyTargetCards(c.hand);
        }

        public override void OnFlip(G g)
        {
            if (g.state.route is Combat c)
            {
                ReapplyModifications(c);
            }
        }

        public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition)
        {
            ReapplyModifications(c);
        }

        public void OnOtherCardDiscardedWhileThisWasInHand(State s, Combat c)
        {
            ReapplyModifications(c);
        }

        public void OnOtherCardDrawnWhileThisWasInHand(State s, Combat c)
        {
            //MainManifest.Instance.Logger.LogInformation($"Modifier card {uuid}:{GetFullDisplayName()} acknowledges with due respect that another card was drawn.");

            ReapplyModifications(c);

            //MainManifest.Instance.Logger.LogInformation($"       Applying to hand {string.Join(", ", c.hand.Select(card => card.GetFullDisplayName()))}");
            //MainManifest.Instance.Logger.LogInformation($"       Applied to {string.Join(", ", currentlyModifiedCards.Select(card => card.GetFullDisplayName()))}");

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

            //MainManifest.Instance.Logger.LogInformation($"Processing {this.GetFullDisplayName()} left cards: {string.Join(", ", leftCards.Select(card => card.GetFullDisplayName()))}      right cards: {string.Join(", ", rightCards.Select(card => card.GetFullDisplayName()))}");

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
                case TargetLocation.ALL_LEFT: return $"{anyEvery} card to the left";
                case TargetLocation.ALL_RIGHT: return $"{anyEvery} card to the right";
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
                case TargetLocation.SINGLE_LEFT: return MainManifest.glossary[$"A{(this.IsFlimsy() ? "Flimsy" : "" )}CardToTheLeft"];
                case TargetLocation.ALL_LEFT: return MainManifest.glossary[$"A{(this.IsFlimsy() ? "Flimsy" : "" )}AllCardsToTheLeft"];
                case TargetLocation.SINGLE_RIGHT: return MainManifest.glossary[$"A{(this.IsFlimsy() ? "Flimsy" : "" )}CardToTheRight"];
                case TargetLocation.ALL_RIGHT: return MainManifest.glossary[$"A{(this.IsFlimsy() ? "Flimsy" : "" )}AllCardsToTheRight"];
                case TargetLocation.NEIGHBORS: return MainManifest.glossary[$"A{(this.IsFlimsy() ? "Flimsy" : "" )}NeighborCards"];
            }

            throw new Exception("Unknown target location " + targetLocation);
        }
        public ExternalSprite GetIconSpriteForTargetLocation()
        {
            var targetLocation = GetTargetLocation();
            switch (targetLocation)
            {
                case TargetLocation.SINGLE_LEFT: return MainManifest.sprites[this.IsFlimsy() ? "icon_Flimsy_Left_Card_Mod" : "icon_card_to_the_left"];
                case TargetLocation.ALL_LEFT: return MainManifest.sprites[this.IsFlimsy() ? "icon_Flimsy_All_Left_Card_Mod" : "icon_all_cards_to_the_left"];
                case TargetLocation.SINGLE_RIGHT: return MainManifest.sprites[this.IsFlimsy() ? "icon_Flimsy_Right_Card_Mod" : "icon_card_to_the_right"];
                case TargetLocation.ALL_RIGHT: return MainManifest.sprites[this.IsFlimsy() ? "icon_Flimsy_All_Right_Card_Mod" : "icon_all_cards_to_the_right"];
                case TargetLocation.NEIGHBORS: return MainManifest.sprites[this.IsFlimsy() ? "icon_Flimsy_Neighbors_Card_Mod" : "icon_card_neighbors"];
            }

            throw new Exception("Unknown target location " + targetLocation);
        }

        public virtual void ApplyMod(Card c) { }
        public virtual TargetLocation GetBaseTargetLocation() { return TargetLocation.SINGLE_LEFT; }
        public virtual bool IsFlimsy() { return false; }
    }
}
