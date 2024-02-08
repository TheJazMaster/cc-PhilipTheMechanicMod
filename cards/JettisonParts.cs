using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace clay.PhilipTheMechanic.Cards;
internal sealed class JettisonParts : Card, IRegisterableCard
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
            cost = 0,
            unplayable = true,
            flippable = true
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<ICardModifier> modifiers = new()
        {
            ModEntry.Instance.Api.MakeMExhaust()
        };

        if (upgrade == Upgrade.A) modifiers.Add(ModEntry.Instance.Api.MakeMAddAction(new AStatus() { status = Status.hermes, statusAmount = 1, targetPlayer = true }, ModEntry.Instance.sprites["icon_sticker_hermes"].Sprite));
        if (upgrade == Upgrade.B) modifiers.Add(ModEntry.Instance.Api.MakeMAddAction(new ASpawn() { thing = new Missile() { missileType = MissileType.normal } }, ModEntry.Instance.sprites["icon_sticker_missile"].Sprite));

        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                new()
                {
                    ModEntry.Instance.Api.MakeMDeleteActions(),
                    ModEntry.Instance.Api.MakeMAddAction(new AStatus() { status = Status.evade, statusAmount = 2, targetPlayer = true }, ModEntry.Instance.sprites["icon_sticker_evade"].Sprite),
                }
            ),
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                modifiers
            )
        };
    }
}
