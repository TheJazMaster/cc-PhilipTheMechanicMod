using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic;

public sealed class ApiImplementation : IPhilipAPI
{
    public IDeckEntry PhilipDeck => ModEntry.Instance.PhilipDeck;
    public IStatusEntry CustomPartsStatus => ModEntry.Instance.CustomPartsStatus;

    public AModifierWrapper MakeAModifierWrapper(IPhilipAPI.CardModifierTarget target, List<CardModifier> modifiers, bool left = false, bool isFlimsy = false, bool overwrites = false) =>
    new() {
        selector = target switch {
            IPhilipAPI.CardModifierTarget.Directional => new SingleDirectionalSelector { left = left },
            IPhilipAPI.CardModifierTarget.Directional_WholeHand => new WholeHandDirectionalSelector { left = left },
            IPhilipAPI.CardModifierTarget.Neighboring => new NeighboringSelector(),
            IPhilipAPI.CardModifierTarget.All => new WholeHandSelector(),
            _ => throw new Exception("Invalid modifier wrapper target specified")
        },
        isFlimsy = isFlimsy,
        overwrites = overwrites,
        modifiers = modifiers,
    };

    public CardModifier MakeMAddAction(CardAction action, Spr? stickerSprite) { return new MAddAction() { action = action, stickerSprite = stickerSprite }; }
    public CardModifier MakeMBuffAttack(int amount) { return new MBuffAttack() { amount = amount }; }
    public CardModifier MakeMAttacksPierce() { return new MAttacksPierce(); }
    public CardModifier MakeMStun() { return new MStun(); }
    public CardModifier MakeMDeleteActions() { return new MDeleteActions(); }
    public CardModifier MakeMPlayTwice() { return new MPlayTwice(); }

    public CardModifier MakeMExhaust() { return new MExhaust(); }
    public CardModifier MakeMRetain() { return new MRetain(); }
    public CardModifier MakeMPlayable() { return new MPlayable(); }
    public CardModifier MakeMRecycle() { return new MRecycle(); }
    public CardModifier MakeMSetEnergyCostToZero() { return new MSetEnergyCostToZero(); }
    public CardModifier MakeMReduceEnergyCost() { return new MReduceEnergyCost(); }
}
