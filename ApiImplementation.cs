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

    public List<IAllowRedrawHook>          AllowRedrawHooks = new();
    public List<(IRedrawCostHook, double)> RedrawCostHooks = new();
    public List<(IOnRedrawHook, double)>   OnRedrawHooks = new();

    public void RegisterAllowRedrawHook(IAllowRedrawHook hook)
    {
        AllowRedrawHooks.Add(hook);
    }

    public void RegisterRedrawCostHook(IRedrawCostHook hook, double priority)
    {
        for (int i = 0; i < RedrawCostHooks.Count; i++)
        {
            if (RedrawCostHooks[i].Item2 < priority)
            {
                RedrawCostHooks.Insert(i, (hook, priority));
            }
        }

        RedrawCostHooks.Add((hook, priority));
    }

    public void RegisterOnRedrawHook(IOnRedrawHook hook, double priority)
    {
        for (int i = 0; i < OnRedrawHooks.Count; i++)
        {
            if (OnRedrawHooks[i].Item2 < priority)
            {
                OnRedrawHooks.Insert(i, (hook, priority));
            }
        }

        OnRedrawHooks.Add((hook, priority));
    }

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
    public ICardModifier MakeMAttacksPierce() { return new MAttacksPierce(); }
    public ICardModifier MakeMStun() { return new MStun(); }
    public ICardModifier MakeMDeleteActions() { return new MDeleteActions(); }
    public ICardModifier MakeMPlayTwice() { return new MPlayTwice(); }

    public ICardModifier MakeMExhaust() { return new MExhaust(); }
    public ICardModifier MakeMRetain() { return new MRetain(); }
    public ICardModifier MakeMUnplayable() { return new MUnplayable(); }
    public ICardModifier MakeMMakePlayable() { return new MMakePlayable(); }
    public ICardModifier MakeMDontExhaust() { return new MDontExhaust(); }
    public ICardModifier MakeMRecycle() { return new MRecycle(); }
    public ICardModifier MakeMSetEnergyCostToZero() { return new MSetEnergyCostToZero(); }
    public ICardModifier MakeMReduceEnergyCost() { return new MReduceEnergyCost(); }
}
