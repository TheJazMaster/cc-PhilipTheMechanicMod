using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Cards;

internal sealed class FrenzyMod : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Frenzy_Mod"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            unplayable = true,
            flippable = upgrade != Upgrade.None
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<ICardModifier> modifiers = new()
        {
            ModEntry.Instance.Api.MakeMAddAction(
                new AAttack() { damage = GetDmg(s, 1) },
                ModEntry.Instance.sprites["icon_sticker_attack"].Sprite
            )
        };

        if (upgrade == Upgrade.A)
            modifiers.Add(
                ModEntry.Instance.Api.MakeMAddAction(
                    new AAttack() { damage = GetDmg(s, 1) },
                    ModEntry.Instance.sprites["icon_sticker_attack"].Sprite
                )
            );

        if (upgrade == Upgrade.B)
            modifiers.Add(
                new Actions.CardModifiers.MBuffAttack() { amount = 1 }
            );

        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                modifiers,
                new() { isFlimsy = true }
            )
        };
    }
}

internal sealed class LoosenScrews : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Loosen_Screws"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            unplayable = true,
            flippable = true
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        // TODO: B
        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                new()
                {
                    ModEntry.Instance.Api.MakeMReduceEnergyCost(),
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AStatus() { status = Status.energyLessNextTurn, statusAmount = 1, targetPlayer = true },
                        ModEntry.Instance.sprites["icon_sticker_energyLessNextTurn"].Sprite
                    )
                }
            )
        };
    }
}

internal sealed class OverfueledEngines : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Overfueled_Engines"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            unplayable = true,
            flippable = upgrade == Upgrade.B
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                new()
                {
                    ModEntry.Instance.Api.MakeMPlayTwice(),
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AAddCard() { amount = 1, card = upgrade == Upgrade.A ? new ColorlessTrash() : new Toxic(), destination = CardDestination.Hand },
                        ModEntry.Instance.sprites["icon_sticker_add_card"].Sprite
                    )
                },
                new() { isFlimsy = true }
            )
        };
    }
}

internal sealed class PiercingMod : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Piercing_Mod"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            unplayable = true,
            flippable = upgrade == Upgrade.A
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<ICardModifier> modifiers = new()
        {
            ModEntry.Instance.Api.MakeMAttacksPierce(),
        };

        if (upgrade == Upgrade.B) modifiers.Add(ModEntry.Instance.Api.MakeMBuffAttack(1));

        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                modifiers
            )
        };
    }
}

internal sealed class InCaseOfEmergency : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Last_Resort"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = true,
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        var addCard = upgrade == Upgrade.B
            ? new AAddCardUpgraded() { amount = 1, card = new OhNo() { upgrade = Upgrade.B }, destination = CardDestination.Hand }
            : new AAddCard() { amount = 1, card = new OhNo(), destination = CardDestination.Hand };

        return new()
        {
            addCard,
            new ADrawCard() { count = 1},
            new AStatus() { status = ModEntry.Instance.Api.RedrawStatus.Status, statusAmount = 1, targetPlayer = true }
        };
    }
}

internal sealed class ShieldingMod : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Shielding_Mod"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            unplayable = true,
            flippable = upgrade != Upgrade.None
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                upgrade == Upgrade.A ? IPhilipAPI.CardModifierTarget.Directional_WholeHand : IPhilipAPI.CardModifierTarget.Directional,
                new()
                {
                    ModEntry.Instance.Api.MakeMAddAction(new AVariableHint_AttackIcon(), null),
                    new MShieldForAttackAmount() { tempShield = upgrade != Upgrade.B }
                }
            )
        };
    }
}

internal sealed class SpareParts : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_philip_default"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new ADrawCard() { count = upgrade == Upgrade.A ? 3 : 2 },

            new AStatus() {
                status = ModEntry.Instance.RedrawStatus.Status,
                targetPlayer = true,
                statusAmount = upgrade == Upgrade.B ? 2 : 1
            },
        };
    }
}

internal sealed class PrecisionMachining : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Precise_Machining"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        int index = c.hand.IndexOf(this);
        bool isCentered = c.hand.Count % 2 == 1 && index == c.hand.Count / 2;

        return new()
        {
            new ACenterOfHandWrapper()
            {
                isCenter = true,
                actions = new()
                {
                    new ADrawCard() { count = 2 },
                    new AStatus() {
                        status = ModEntry.Instance.RedrawStatus.Status,
                        targetPlayer = true,
                        statusAmount = upgrade == Upgrade.B ? 4 : 2
                    },
                },
                disabled = (!isCentered && c != DB.fakeCombat)
            },
            new ACenterOfHandWrapper()
            {
                isCenter = false,
                actions = new()
                {
                    new AStatus() {
                        status = ModEntry.Instance.RedrawStatus.Status,
                        targetPlayer = true,
                        statusAmount = upgrade == Upgrade.A ? 2 : 1
                    },
                },
                disabled = (isCentered && c != DB.fakeCombat)
            }
        };
    }
}