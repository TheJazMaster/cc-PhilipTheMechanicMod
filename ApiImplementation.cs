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
    public IStatusEntry RedrawStatus => ModEntry.Instance.RedrawStatus;
    public IStatusEntry CustomPartsStatus => ModEntry.Instance.CustomPartsStatus;

    public CardAction MakeAModifierWrapper(IPhilipAPI.CardModifierTarget target, List<ICardModifier> modifiers, IPhilipAPI.AModifierWrapperMeta meta)
    {
        switch (target)
        {
            case IPhilipAPI.CardModifierTarget.Directional:
                return new ADirectionalCardModifierWrapper()
                {
                    isFlimsy = meta.isFlimsy,
                    modifiers = modifiers,
                    left = meta.direction == IPhilipAPI.CardModifierTargetDirection.LEFT
                };
            case IPhilipAPI.CardModifierTarget.Directional_WholeHand:
                return new AWholeHandDirectionalCardsModifierWrapper()
                {
                    isFlimsy = meta.isFlimsy,
                    modifiers = modifiers,
                    left = meta.direction == IPhilipAPI.CardModifierTargetDirection.LEFT
                };
            case IPhilipAPI.CardModifierTarget.Neighboring:
                return new ANeighboringCardsModifierWrapper()
                {
                    isFlimsy = meta.isFlimsy,
                    modifiers = modifiers,
                };
        }

        throw new Exception("Invalid modifier wrapper target specified");
    }

    public ICardModifier MakeMAddAction(CardAction action, Spr? stickerSprite) { return new MAddAction() { action = action, stickerSprite = stickerSprite }; }
    public ICardModifier MakeMBuffAttack(int amount) { return new MBuffAttack() { amount = amount }; }
    public ICardModifier MakeMDeleteActions() { return new MDeleteActions(); }
    public ICardModifier MakeMExhaust() { return new MExhaust(); }
    public ICardModifier MakeMRetain() { return new MRetain(); }
    public ICardModifier MakeMUnplayable() { return new MUnplayable(); }
}
