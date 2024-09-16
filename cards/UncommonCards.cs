using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Cards;

internal sealed class FrenzyMod : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Frenzy_Mod"];

    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            unplayable = true,
            flippable = true
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        List<CardModifier> modifiers = [
			ModEntry.Instance.Api.MakeMAddAction(
                new AAttack() { damage = GetDmg(s, 1) },
                ModEntry.Instance.sprites["icon_sticker_attack"]
            )
        ];

        if (upgrade == Upgrade.A)
            modifiers.Add(
                ModEntry.Instance.Api.MakeMAddAction(
                    new AAttack() { damage = GetDmg(s, 1) },
                    ModEntry.Instance.sprites["icon_sticker_attack"]
                )
            );

        if (upgrade == Upgrade.B)
            modifiers.Add(
                new MBuffAttack() { amount = 1 }
            );

        return [
			new ASingleDirectionalCardModifierWrapper {
                modifiers = modifiers,
                isFlimsy = true
            }
        ];
    }
}

internal sealed class LoosenScrews : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Loosen_Screws"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        CardModifier energyReductionMod = upgrade == Upgrade.B
            ? new MSetEnergyCostToZero()
            : new MReduceEnergyCost();

        var penaltyMod = new MAddAction {
            action = new AAddCard() {
                card = upgrade == Upgrade.A ? new TrashFumes() : new ColorlessTrash(),
                amount = upgrade == Upgrade.B ? 2 : 1
            },
            goLast = true,
            stickerSprite = ModEntry.Instance.sprites["icon_sticker_add_card"]
        };
        return [
			new ANeighboringCardsModifierWrapper {
                modifiers = [
                    energyReductionMod,
                    penaltyMod
                ]
            }
        ];
    }
}

internal sealed class MarauderMod : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Piercing_Mod"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            unplayable = true,
            flippable = upgrade == Upgrade.A
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        List<CardModifier> modifiers = [
			ModEntry.Instance.Api.MakeMAttacksPierce(),
            ModEntry.Instance.Api.MakeMStun()
        ];

        if (upgrade == Upgrade.B) modifiers.Add(ModEntry.Instance.Api.MakeMBuffAttack(1));

        return [
			new ASingleDirectionalCardModifierWrapper {
                modifiers = modifiers
            },
			new ASingleDirectionalCardModifierWrapper {
                modifiers = [new MAddAction {
                    action = new AMove {
                        dir = 1,
                        targetPlayer = true
                    }
                }]
            }
        ];
    }
}

internal sealed class SignalBoost : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Overfueled_Engines"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            flippable = upgrade == Upgrade.B
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        return [
			new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    ModEntry.Instance.Api.MakeMPlayTwice(),
                    new MAddAction {
                        action = new AAddCard() {
                            amount = upgrade == Upgrade.A ? 1 : 2,
                            card = new TrashUnplayable(),
                            destination = CardDestination.Hand
                        },
                        goLast = true, 
                        stickerSprite = ModEntry.Instance.sprites["icon_sticker_add_card"]
                    }
                ],
                isFlimsy = true
            }
        ];
    }
}

internal sealed class InCaseOfEmergency : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Last_Resort"];

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

        return [
			addCard,
            new ADrawCard() { count = 1},
            new AStatus() { status = ModEntry.Instance.RedrawStatus, statusAmount = 1, targetPlayer = true }
        ];
    }
}

internal sealed class ShieldingMod : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Shielding_Mod"];

    public override CardData GetData(State state) => new() {
        cost = 1
    };

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => [
        ModEntry.Instance.Api.MakeAModifierWrapper(
            upgrade == Upgrade.A ? IPhilipAPI.CardModifierTarget.Directional_WholeHand : IPhilipAPI.CardModifierTarget.Directional,
            [
                new MAddAction() {
                    action = new AVariableHintAttack(),
                }
            ]
        ), ModEntry.Instance.Api.MakeAModifierWrapper(
            upgrade == Upgrade.A ? IPhilipAPI.CardModifierTarget.Directional_WholeHand : IPhilipAPI.CardModifierTarget.Directional,
            [
                new MShieldForAttackAmount() { tempShield = upgrade != Upgrade.B }
            ]
        )
    ];

    public override List<CardAction> GetOtherActions(State s, Combat c) => [
        new AStatus {
            status = Status.tempShield,
            statusAmount = 2,
            targetPlayer = true
        }
    ];
}

internal sealed class PrecisionMachining : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.uncommon;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Precise_Machining"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        int index = c.hand.IndexOf(this);
        bool isCentered = c.hand.Count % 2 == 1 && index == c.hand.Count / 2;
        return [
			ACenterOfHandWrapper.Make(false, [
					new AStatus() {
                        status = ModEntry.Instance.RedrawStatus,
                        targetPlayer = true,
                        statusAmount = upgrade == Upgrade.A ? 2 : 1,
                    },
                ],
                isCentered && c != DB.fakeCombat
            ),
            ACenterOfHandWrapper.Make(true, [
					new ADrawCard() { count = upgrade == Upgrade.B ? 3 : 2 },
                    new AStatus() {
                        status = ModEntry.Instance.RedrawStatus,
                        targetPlayer = true,
                        statusAmount = 2
                    },
                ],
                !isCentered && c != DB.fakeCombat
            ),
        ];
    }
}