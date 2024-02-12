﻿using Nickel;
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

    public interface ICardModifier
    {
        bool RequestsStickyNote() { return false; }
        bool MandatesStickyNote() { return false; }
        Spr? GetSticker(State s) { return null; }
        Icon? GetIcon(State s) { return null; }
        List<Tooltip> GetTooltips(State s) { return new(); }
        List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card) { return actions; }
        CardData TransformData(CardData data, State s, Combat c) { return data; }
    }
}
