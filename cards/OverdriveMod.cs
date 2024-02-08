using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace clay.PhilipTheMechanic.Cards;
internal sealed class OverdriveMod : Card, IDemoCard
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("OverdriveMod", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.PhilipDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [ Upgrade.A, Upgrade.B ]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "OverdriveMod", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        CardData data = new CardData()
        {
            cost = 1,
        };
        return data;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AAdjacentCardModifierWrapper()
            {
                isFlimsy = true,
                modifiers = new()
                {
                    new MBuffAttack() { amount = 1 },
                }
            },
            new AAttack() { damage = GetDmg(s, 1) }
        };
    }
}
