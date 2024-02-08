using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace clay.PhilipTheMechanic.Cards;
internal sealed class OverdriveMod : Card, IRegisterableCard
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
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", cardType.Name, "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            flippable = upgrade != Upgrade.None
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new ADirectionalCardModifierWrapper()
            {
                modifiers = new()
                {
                    new MBuffAttack() { amount = upgrade == Upgrade.A ? 2 : 1 },
                }
            },
            new AAttack() { damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1) }
        };
    }
}
