using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Cards;


internal sealed class RecycleParts : Card
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Recycle_Parts"];

    public override CardData GetData(State state) => new()
    {
        cost = upgrade == Upgrade.A ? 0 : 1
    };

    public override List<CardAction> GetActions(State s, Combat c) =>
    [
        new AStatus() {
            status = ModEntry.Instance.RedrawStatus,
            targetPlayer = true,
            statusAmount = upgrade == Upgrade.B ? 2 : 1
        },
        new AStatus() {
            status = Status.tempShield,
            targetPlayer = true,
            statusAmount = 2
        }
    ];
}


internal sealed class OpenBayDoors : ModifierCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Open_Bay_Doors"];

    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            retain = upgrade == Upgrade.A
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        List<CardModifier> modifiers =
		[
			ModEntry.Instance.Api.MakeMExhaust(),
            ModEntry.Instance.Api.MakeMReduceEnergyCost()
        ];

        if (upgrade == Upgrade.B)
        {
            modifiers.Add
            (
                ModEntry.Instance.Api.MakeMAddAction
                (
                    new ASpawn() { thing = new Missile() { missileType = MissileType.normal } },
                    ModEntry.Instance.sprites["icon_sticker_missile_normal"]
                )
            );
        }

        return [
            new AModifierWrapper {
                selector = new WholeHandDirectionalSelector { left = upgrade != Upgrade.A },
                modifiers = modifiers,
            }
        ];
    }
}


internal sealed class StunMod : ModifierCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Stun_Mod"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            flippable = true,
            unplayable = upgrade != Upgrade.B
        };
    }

    public override List<CardAction> GetOtherActions(State s, Combat c) => upgrade switch {
        Upgrade.B => [
            new ADrawCard { count = 1 },
            new AAttack() { damage = GetDmg(s, 0), stunEnemy = true }
        ],
        _ => [
            new ADrawCard { count = 1 }
        ]
    };

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => upgrade switch {
        Upgrade.A => [
            new AModifierWrapper {
                selector = new SingleDirectionalSelector(),
                modifiers = [ModEntry.Instance.Api.MakeMStun(), ModEntry.Instance.Api.MakeMAddAction(new AStatus() { status = Status.stunCharge, targetPlayer = true, statusAmount = 1 }, ModEntry.Instance.sprites["icon_sticker_stun"])]
            }
        ],
        _ => [
            new AModifierWrapper {
                selector = new SingleDirectionalSelector(),
                modifiers = [ModEntry.Instance.Api.MakeMStun()]
            }
        ]
    };
}

internal sealed class PiercingMod : ModifierCard
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
        ];

        if (upgrade == Upgrade.B) modifiers.Add(ModEntry.Instance.Api.MakeMBuffAttack(1));

        return [
			new AModifierWrapper {
                selector = new NeighboringSelector(),
                modifiers = modifiers
            }
        ];
    }
}

internal sealed class DisableSafeties : ModifierCard
{
    public static Rarity GetRarity() => Rarity.rare;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Last_Resort"];

    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            unplayable = upgrade == Upgrade.B
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        if (upgrade == Upgrade.B)
        {
            return [
				new AModifierWrapper {
                    selector = new NeighboringSelector(),
                    modifiers = [ModEntry.Instance.Api.MakeMAddAction(
                            new AAttack() { damage = GetDmg(s, 1) },
                            ModEntry.Instance.sprites["icon_sticker_attack"]
                        )
                    ]
                },
                new AModifierWrapper {
                    selector = new NeighboringSelector(),
                    modifiers = [
                        ModEntry.Instance.Api.MakeMAddAction(
                            new AStatus() { status = Status.heat, targetPlayer = true, statusAmount = 1 },
                            ModEntry.Instance.sprites["icon_sticker_heat"]
                        )
                    ]
                }
            ];
        }

        return [
			new AModifierWrapper {
                selector = new WholeHandDirectionalSelector(),
                modifiers = [
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AAttack() { damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1) },
                        ModEntry.Instance.sprites["icon_sticker_attack"]
                    ),
                ],
                isFlimsy = true
            },
            new AModifierWrapper {
                selector = new WholeHandDirectionalSelector(),
                modifiers = [
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AAttack() { damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1) },
                        ModEntry.Instance.sprites["icon_sticker_attack"]
                    ),
                ],
                isFlimsy = true
            },
            new AModifierWrapper {
                selector = new WholeHandDirectionalSelector(),
                modifiers = [
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AStatus() { status = Status.heat, targetPlayer = true, statusAmount = upgrade == Upgrade.A ? 3 : 2 },
                        ModEntry.Instance.sprites["icon_sticker_heat"]
                    )
                ],
                isFlimsy = true
            }
        ];
    }
}