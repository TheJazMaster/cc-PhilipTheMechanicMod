using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Cards;


internal sealed class OverdriveMod : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Overdrive_Mod"];
    public override CardData GetData(State state)
    {
        return new() {
            cost = 1,
            flippable = upgrade == Upgrade.A
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => [
        new AModifierWrapper {
            selector = new SingleDirectionalSelector(),
            modifiers = [
                new MBuffAttack() { amount = upgrade == Upgrade.B ? 3 : 1 },
            ],
            isFlimsy = upgrade == Upgrade.B
        }
    ];

    public override List<CardAction> GetOtherActions(State s, Combat c) => [
        new AStatus {
            status = Status.tempShield,
            statusAmount = upgrade == Upgrade.A ? 3 : 2,
            targetPlayer = true
        }
    ];
}

internal sealed class TinkerShot : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Stun_Mod"];
	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 0 : 1
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AAttack
		{
			damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1)
		},
		new AStatus
		{
			status = ModEntry.Instance.RedrawStatus,
			statusAmount = upgrade == Upgrade.B ? 1 : 2,
			targetPlayer = true
		}
	];
}

internal sealed class DrawMod : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Permanence_Mod"];
    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            unplayable = true
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => [
        new AModifierWrapper {
            selector = upgrade == Upgrade.A ? new WholeHandDirectionalSelector { left = true } : new SingleDirectionalSelector { left = true },
            modifiers = [
                new MAddAction() {
                    action = new ADrawCard() { count = 1 },
                    stickerSprite = ModEntry.Instance.sprites["icon_sticker_draw"]
                },
            ],
        },
        new AModifierWrapper {
            selector = new SingleDirectionalSelector(),
            modifiers = [
                new MAddAction() {
                    action = new ADrawCard() { count = 1 },
                    stickerSprite = ModEntry.Instance.sprites["icon_sticker_draw"]
                },
            ],
            isFlimsy = true
        }
    ];
}

internal sealed class DuctTapeAndDreams : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Duct_Tape_and_Dreams"];
    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            flippable = upgrade == Upgrade.A,
            retain = true,
        };
    }

    public override List<CardAction> GetOtherActions(State s, Combat c) => [];

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => upgrade switch {
        Upgrade.B => [
            new AModifierWrapper {
                selector = new NeighboringSelector(),
                modifiers = [new MRetain(), new MPlayable { value = false }]
            }
        ],
        _ => [
            new AModifierWrapper {
                selector = new SingleDirectionalSelector(),
                modifiers = [new MRetain()]
            }
        ],
    };
}

internal sealed class JettisonParts : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Last_Resort"];
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            unplayable = true,
            flippable = true
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) {
        List<CardModifier> modifiers = [
            new MAddAction {
                action = new AStatus() { status = Status.evade, statusAmount = 1, targetPlayer = true },
                stickerSprite = ModEntry.Instance.sprites["icon_sticker_evade"]
            }
        ];

        if (upgrade == Upgrade.A) modifiers.Add(ModEntry.Instance.Api.MakeMAddAction(new AStatus() { status = Status.hermes, statusAmount = 1, targetPlayer = true }, ModEntry.Instance.sprites["icon_sticker_hermes"]));
        if (upgrade == Upgrade.B) modifiers.Add(ModEntry.Instance.Api.MakeMAddAction(new ASpawn() { thing = new Missile() { missileType = MissileType.normal } }, ModEntry.Instance.sprites["icon_sticker_missile_normal"]));

        return [
			new AModifierWrapper {
                selector = new SingleDirectionalSelector(),
                modifiers = modifiers,
                overwrites = true
            },
            new AModifierWrapper {
                selector = new WholeHandDirectionalSelector(),
                modifiers = [
                    new MExhaust()
                ]
            }
        ];
    }
}

internal sealed class Oops : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Oops"];

    public override CardData GetData(State state) => new() {
        cost = 0,
        retain = upgrade == Upgrade.B
    };

	public override List<CardAction> GetActions(State s, Combat c) => [
        new AMove {
            dir = upgrade == Upgrade.A ? 3 : 2,
            isRandom = true,
            targetPlayer = true
        },
        new AShuffleHand()
    ];
}

internal sealed class ReduceReuse : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Recycle_Parts"];

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            flippable = true
        };
    }

    public override List<CardAction> GetOtherActions(State s, Combat c) => upgrade switch {
        Upgrade.A => [
            new AStatus {
                status = ModEntry.Instance.RedrawStatus, targetPlayer = true, statusAmount = 2 
            },
            new ADrawCard() { count = 1 }
        ],
        _ => [
            new ADrawCard() { count = 1 }
        ]
    };      
            
    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => upgrade switch
    {
        Upgrade.B => [
            new AModifierWrapper {
                selector = new SingleDirectionalSelector(),
                modifiers = [
                    new MRecycle(),
                    new MSetEnergyCostToZero(),
                    new MExhaust { value = false }
                ],
                overwrites = true
            }
        ],
        _ => [
			new AModifierWrapper {
                selector = new SingleDirectionalSelector(),
                modifiers = [new MRecycle()]
            }
        ]
    };
}

internal sealed class ToolsOfTheTrade : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_philip_default"];

    public override CardData GetData(State state) => new() {
        cost = 1
    };

    public override List<CardAction> GetActions(State s, Combat c) => [
        new ADrawCard() {
            count = upgrade == Upgrade.A ? 4 : 2
        },
        new AStatus() {
            status = Status.drawNextTurn,
            targetPlayer = true,
            statusAmount = upgrade == Upgrade.B ? 2 : 1
        },
        new AStatus() {
            status = ModEntry.Instance.RedrawStatus,
            targetPlayer = true,
            statusAmount = upgrade == Upgrade.B ? 2 : 1
        },
    ];
}

internal sealed class Hotwire : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Permanence_Mod"];
    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            unplayable = true,
            flippable = upgrade == Upgrade.A
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => upgrade switch {
        Upgrade.A => [
            new AModifierWrapper {
                selector = new SingleDirectionalSelector(),
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? -1 : 1,
                            targetPlayer = true
                        },
                        stickerSprite = ModEntry.Instance.sprites[flipped ? "icon_sticker_move_left" : "icon_sticker_move_right"]
                    }
                ],
            }
        ],
        _ => [
            new AModifierWrapper {
                selector = upgrade == Upgrade.B ? new WholeHandDirectionalSelector() : new SingleDirectionalSelector(),
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? -1 : 1,
                            targetPlayer = true
                        },
                        stickerSprite = ModEntry.Instance.sprites[flipped ? "icon_sticker_move_left" : "icon_sticker_move_right"]
                    }
                ],
            },
            new AModifierWrapper {
                selector = upgrade == Upgrade.B ? new WholeHandDirectionalSelector { left = true } : new SingleDirectionalSelector { left = true },
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? 1 : -1,
                            targetPlayer = true
                        },
                        stickerSprite = ModEntry.Instance.sprites[flipped ? "icon_sticker_move_right" : "icon_sticker_move_left"]
                    }
                ],
            }
        ],
    };
}