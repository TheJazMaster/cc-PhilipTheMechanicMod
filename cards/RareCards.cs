using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Cards;

internal sealed class BlackMarketParts : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.rare;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Black_Market_Parts"];

    public override CardData GetData(State state)
    {
        return new() {
            cost = 1,
            unplayable = true,
            flippable = true
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        return [
			new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MDeleteActions(),
                    new MPlayable(),
                    new MExhaust(),
                ]
            },
			new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction {
                        action = new AAddCardUpgraded() {
                            card = new UraniumRound() { upgrade = upgrade }, destination = CardDestination.Hand
                        },
                        stickerSprite = ModEntry.Instance.sprites["icon_sticker_add_card"]
                    }
                ]
            }

        ];
    }
}

internal sealed class EmergencyTraining : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.rare;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Emergency_Training"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 2 : 3,
            exhaust = true,
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        var addCardAction = upgrade == Upgrade.B
            ? new AAddCardUpgraded() { card = new ImpromptuBlastShield() { upgrade = Upgrade.B }, destination = CardDestination.Hand }
            : new AAddCard() { card = new ImpromptuBlastShield(), destination = CardDestination.Hand };
        return [
			new ADiscard(),
            new AStatus() {
                status = ModEntry.Instance.RedrawStatus,
                targetPlayer = true,
                statusAmount = 5
            },
            addCardAction,
            new ADrawCard() { count = 4 }
        ];
    }
}

internal sealed class NoStockParts : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.rare;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_philip_default"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = upgrade != Upgrade.B,
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return [
			new AStatus() {
                status = ModEntry.Instance.CustomPartsStatus.Status,
                targetPlayer = true,
                statusAmount = 1
            },
        ];
    }
}

internal sealed class NanobotInfestation : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.rare;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Nanobot_Infestation"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            unplayable = true,
            flippable = true,
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        var addCardAction = new AAddCardUpgraded()
        {
            card = new Nanobots() { upgrade = upgrade == Upgrade.A ? Upgrade.A : Upgrade.None },
            amount = upgrade == Upgrade.B ? 9 : 1,
            destination = CardDestination.Hand
        };

        List<AModifierWrapper> actions = [
			new ASingleDirectionalCardModifierWrapper {
                modifiers = upgrade == Upgrade.B ? [
                    ModEntry.Instance.Api.MakeMPlayTwice(),
                    ModEntry.Instance.Api.MakeMPlayTwice(),
                    ModEntry.Instance.Api.MakeMSetEnergyCostToZero(),
                ] : [
                    ModEntry.Instance.Api.MakeMPlayTwice(),
                    ModEntry.Instance.Api.MakeMSetEnergyCostToZero(),
                ],
                isFlimsy = true
            },
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction {
                        action = addCardAction,
                        goLast = true,
                        stickerSprite = ModEntry.Instance.sprites["icon_sticker_add_card"]
                    },
                ],
                isFlimsy = true
            }
        ];

        return actions;
    }
}

internal sealed class Repeater : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.rare;
    public static Spr GetArt() => StableSpr.cards_ThinkTwice;

    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            unplayable = true,
            flippable = upgrade == Upgrade.A,
            artTint = null
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => upgrade switch {
        Upgrade.B => [
			new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MInfinite(),
                    new MAddAction {
                        action = new AShuffleHand(),
                        stickerSprite = ModEntry.Instance.sprites["icon_sticker_shuffle_hand"]
                    }
                ]
            }
        ],
        _ => [
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MInfinite()
                ],
                isFlimsy = true
            }
        ]
    };
}
