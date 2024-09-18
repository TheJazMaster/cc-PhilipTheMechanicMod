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
        new ASingleDirectionalCardModifierWrapper {
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

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => upgrade switch {
        Upgrade.A => [
            new AWholeHandDirectionalCardsModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new ADrawCard() { count = 1 }
                    },
                ],
                left = true
            },
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new ADrawCard() { count = 1 }
                    },
                ],
                isFlimsy = true
            },
        ],
        _ => [
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new ADrawCard() { count = 1 }
                    },
                ],
                left = true
            },
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new ADrawCard() { count = upgrade == Upgrade.B ? 3 : 1 }
                    },
                ],
                isFlimsy = true
            },
        ]
    };
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
            new ANeighboringCardsModifierWrapper {
                modifiers = [new MRetain(), new MPlayable { value = false }]
            }
        ],
        _ => [
            new ASingleDirectionalCardModifierWrapper {
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
                action = new AStatus() { status = Status.evade, statusAmount = 2, targetPlayer = true },
                stickerSprite = ModEntry.Instance.sprites["icon_sticker_evade"]
            }
        ];

        if (upgrade == Upgrade.A) modifiers.Add(ModEntry.Instance.Api.MakeMAddAction(new AStatus() { status = Status.hermes, statusAmount = 1, targetPlayer = true }, ModEntry.Instance.sprites["icon_sticker_hermes"]));
        if (upgrade == Upgrade.B) modifiers.Add(ModEntry.Instance.Api.MakeMAddAction(new ASpawn() { thing = new Missile() { missileType = MissileType.normal } }, ModEntry.Instance.sprites["icon_sticker_missile_normal"]));

        return [
			new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MDeleteActions(),
                    new MPlayable(),
                    new MExhaust(),
                ]
            },
			new ASingleDirectionalCardModifierWrapper {
                modifiers = modifiers
            }
        ];
    }
}

internal sealed class Oops : ModifierCard, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Oops"];

    public override CardData GetData(State state) => new() {
        cost = 0,
        unplayable = true,
        flippable = upgrade == Upgrade.B
    };

    private Spr GetSticker() => ModEntry.Instance.sprites[upgrade switch {
        Upgrade.B => flipped ? "icon_sticker_move_left" : "icon_sticker_move_right",
        _ => "icon_sticker_move_random"
    }];

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => [
        ModEntry.Instance.Api.MakeAModifierWrapper
        (
            upgrade == Upgrade.A ? IPhilipAPI.CardModifierTarget.Directional_WholeHand : IPhilipAPI.CardModifierTarget.Neighboring,
            [
                ModEntry.Instance.Api.MakeMAddAction
                (
                    new AStatus() {
                        status = ModEntry.Instance.RedrawStatus,
                        targetPlayer = true,
                        statusAmount = upgrade == Upgrade.A ? 2 : 1,
                    },
                    ModEntry.Instance.sprites["icon_sticker_redraw"]
                ),
                ModEntry.Instance.Api.MakeMAddAction
                (
                    new AMove() {
                        dir = (upgrade == Upgrade.B && flipped) ? -1 : 1,
                        isRandom = upgrade != Upgrade.B,
                        targetPlayer = true
                    },
                    GetSticker()
                )
            ]
        )
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
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MRecycle(),
                    new MPlayable()
                ]
            }
        ],
        _ => [
			new ASingleDirectionalCardModifierWrapper {
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
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? -1 : 1,
                            targetPlayer = true
                        }
                    }
                ],
            }
        ],
        Upgrade.B => [
            new AWholeHandDirectionalCardsModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? -1 : 1,
                            targetPlayer = true
                        }
                    }
                ],
            },
            new AWholeHandDirectionalCardsModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? 1 : -1,
                            targetPlayer = true
                        }
                    }
                ],
                left = true
            }
        ],
        _ => [
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? -1 : 1,
                            targetPlayer = true
                        }
                    }
                ],
            },
            new ASingleDirectionalCardModifierWrapper {
                modifiers = [
                    new MAddAction() {
                        action = new AMove() {
                            dir = flipped ? 1 : -1,
                            targetPlayer = true
                        }
                    }
                ],
                left = true
            }
        ],
    };
}