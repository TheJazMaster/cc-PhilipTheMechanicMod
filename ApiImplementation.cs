using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic;

public sealed class ApiImplementation : IPhilipAPI
{
    public IDeckEntry PhilipDeck => ModEntry.Instance.PhilipDeck;
    public IStatusEntry CustomPartsStatus => ModEntry.Instance.CustomPartsStatus;

    public AModifierWrapper MakeAModifierWrapper(IPhilipAPI.CardModifierTarget target, List<CardModifier> modifiers, bool isFlimsy = false, bool left = false)
    {
        switch (target)
        {
            case IPhilipAPI.CardModifierTarget.Directional:
                return new ASingleDirectionalCardModifierWrapper()
                {
                    isFlimsy = isFlimsy,
                    modifiers = modifiers,
                    left = left
                };
            case IPhilipAPI.CardModifierTarget.Directional_WholeHand:
                return new AWholeHandDirectionalCardsModifierWrapper()
                {
                    isFlimsy = isFlimsy,
                    modifiers = modifiers,
                    left = left
                };
            case IPhilipAPI.CardModifierTarget.Neighboring:
                return new ANeighboringCardsModifierWrapper()
                {
                    isFlimsy = isFlimsy,
                    modifiers = modifiers,
                };
        }

        throw new Exception("Invalid modifier wrapper target specified");
    }

    public CardModifier MakeMAddAction(CardAction action, Spr? stickerSprite) { return new MAddAction() { action = action, stickerSprite = stickerSprite }; }
    public CardModifier MakeMBuffAttack(int amount) { return new MBuffAttack() { amount = amount }; }
    public CardModifier MakeMAttacksPierce() { return new MAttacksPierce(); }
    public CardModifier MakeMStun() { return new MStun(); }
    public CardModifier MakeMDeleteActions() { return new MDeleteActions(); }
    public CardModifier MakeMPlayTwice() { return new MPlayTwice(); }

    public CardModifier MakeMExhaust() { return new MExhaust(); }
    public CardModifier MakeMRetain() { return new MRetain(); }
    public CardModifier MakeMPlayable() { return new MPlayable(); }
    public CardModifier MakeMDontExhaust() { return new MDontExhaust(); }
    public CardModifier MakeMRecycle() { return new MRecycle(); }
    public CardModifier MakeMSetEnergyCostToZero() { return new MSetEnergyCostToZero(); }
    public CardModifier MakeMReduceEnergyCost() { return new MReduceEnergyCost(); }
}
