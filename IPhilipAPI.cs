using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic
{
    public interface IPhilipAPI
    {
        IDeckEntry PhilipDeck { get; }
        IStatusEntry RedrawStatus { get; }
        IStatusEntry CustomPartsStatus { get; }

        void RegisterAllowRedrawHook(IAllowRedrawHook hook); // Redraw will be enabled on a card if ANY hook allows it
        void RegisterRedrawCostHook(IRedrawCostHook hook, double priority);
        void RegisterOnRedrawHook(IOnRedrawHook hook, double priority);

        enum CardModifierTarget
        {
            Directional,
            Directional_WholeHand,
            Neighboring
        }

        enum CardModifierTargetDirection
        {
            RIGHT,
            LEFT
        }

        struct AModifierWrapperMeta
        {
            public bool isFlimsy;
            public CardModifierTargetDirection direction;
        }

        CardAction MakeAModifierWrapper(CardModifierTarget target, List<ICardModifier> modifiers, AModifierWrapperMeta meta = default);

        ICardModifier MakeMAddAction(CardAction action, Spr? stickerSprite);
        ICardModifier MakeMBuffAttack(int amount);
        ICardModifier MakeMAttacksPierce();
        ICardModifier MakeMStun();
        ICardModifier MakeMDeleteActions();
        ICardModifier MakeMPlayTwice();

        ICardModifier MakeMExhaust();
        ICardModifier MakeMRetain();
        ICardModifier MakeMUnplayable();
        ICardModifier MakeMMakePlayable();
        ICardModifier MakeMDontExhaust();
        ICardModifier MakeMRecycle();
        ICardModifier MakeMSetEnergyCostToZero();
        ICardModifier MakeMReduceEnergyCost();
    }

    public interface IAllowRedrawHook
    {
        bool AllowRedraw(Card card, State state, Combat combat);
    }
    public interface IRedrawCostHook
    {
        int RedrawCost(int currentCost, Card card, State state, Combat combat);
    }
    public interface IOnRedrawHook
    {
        void OnRedraw(Card card, State state, Combat combat);
    }

    public interface ICardModifier
    {
        string DialogueTag { get; }
        double Priority { get; }
        bool RequestsStickyNote() { return false; }
        bool MandatesStickyNote() { return false; }
        Spr? GetSticker(State s) { return null; }
        Icon? GetIcon(State s) { return null; }
        List<Tooltip> GetTooltips(State s) { return new(); }
        List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering) { return actions; }
        CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering) { return data; }
    }
}
