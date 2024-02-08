using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace clay.PhilipTheMechanic.Cards;
internal sealed class RecycleParts : Card, IDemoCard
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
            cost = upgrade == Upgrade.A ? 0 : 1 
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AStatus() {
                status = ModEntry.Instance.RedrawStatus.Status,
                targetPlayer = true,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                mode = Enum.Parse<AStatusMode>("Add"),
            },
            new AStatus() {
                status = Enum.Parse<Status>("tempShield"),
                targetPlayer = true,
                statusAmount = 2,
                mode = Enum.Parse<AStatusMode>("Add"),
            }
        };
    }
}
