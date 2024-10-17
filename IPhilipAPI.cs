using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
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
        IStatusEntry CustomPartsStatus { get; }

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

        AModifierWrapper MakeAModifierWrapper(CardModifierTarget target, List<CardModifier> modifiers, bool isFlimsy = false, bool left = false);

        CardModifier MakeMAddAction(CardAction action, Spr? stickerSprite);
        CardModifier MakeMBuffAttack(int amount);
        CardModifier MakeMAttacksPierce();
        CardModifier MakeMStun();
        CardModifier MakeMDeleteActions();
        CardModifier MakeMPlayTwice();

        CardModifier MakeMExhaust();
        CardModifier MakeMRetain();
        CardModifier MakeMPlayable();
        CardModifier MakeMRecycle();
        CardModifier MakeMSetEnergyCostToZero();
        CardModifier MakeMReduceEnergyCost();
    }

    public abstract class CardModifier
    {
        public List<int> flimsyUuids = [];
        public abstract double Priority { get; }
        virtual public bool RequestsStickyNote() { return false; }
        virtual public bool MandatesStickyNote() { return false; }
        virtual public Spr? GetSticker(State s) { return null; }
        // virtual public Icon? GetIcon(State s) { return null; }
        public abstract CardAction GetActionForRendering(State s);
        virtual public List<Tooltip> GetTooltips(State s) { return []; }
    }

    public interface ICardActionModifier
    {
        List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering) { return actions; }
    }

    public interface ICardDataModifier
    {
        CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering) { return data; }
    }
}
