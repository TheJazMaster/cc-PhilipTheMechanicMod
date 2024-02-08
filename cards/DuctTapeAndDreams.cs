using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace clay.PhilipTheMechanic.Cards;

internal sealed class DuctTapeAndDreams : Card, IRegisterableCard
{
    public static void Register(IModHelper helper)
    {
        var cardType = MethodBase.GetCurrentMethod()!.DeclaringType!;
        helper.Content.Cards.RegisterCard(cardType.Name, new()
        {
            CardType = cardType,
            Meta = new()
            {
                deck = ModEntry.Instance.PhilipDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [ Upgrade.A, Upgrade.B ]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", cardType.Name, "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            flippable = upgrade != Upgrade.None,
            retain = true,
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                new() { ModEntry.Instance.Api.MakeMRetain() }
            )
        };
    }
}
