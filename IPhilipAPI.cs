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

        CardAction MakeAModifierWrapper(CardModifierTarget target, List<ICardModifier> modifiers, bool isFlimsy = false, CardModifierTargetDirection direction = CardModifierTargetDirection.RIGHT);

        ICardModifier MakeMBuffAttack(int amount);
        ICardModifier MakeMExhaust();
        ICardModifier MakeMRetain();
    }

    public interface ICardModifier
    {
        Spr? GetSticker(State s) { return null; }
        Icon? GetIcon(State s) { return null; }
        List<Tooltip> GetTooltips(State s) { return new(); }
        List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c) { return actions; }
        CardData TransformData(CardData data, State s, Combat c) { return data; }
    }
}
