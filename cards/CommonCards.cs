using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Cards;


internal sealed class RecycleParts : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Recycle_Parts"].Sprite;

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
                statusAmount = upgrade == Upgrade.B ? 2 : 1
            },
            new AStatus() {
                status = Enum.Parse<Status>("tempShield"),
                targetPlayer = true,
                statusAmount = 2
            }
        };
    }
}


internal sealed class OverdriveMod : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Overdrive_Mod"].Sprite;
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
                    new Actions.CardModifiers.MBuffAttack() { amount = upgrade == Upgrade.A ? 2 : 1 },
                }
            },
            new AAttack() { damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1) }
        };
    }
}


internal sealed class DuctTapeAndDreams : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Duct_Tape_and_Dreams"].Sprite;
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            flippable = upgrade == Upgrade.A,
            retain = true,
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        if (upgrade == Upgrade.B)
        {
            return new()
            {
                ModEntry.Instance.Api.MakeAModifierWrapper
                (
                    IPhilipAPI.CardModifierTarget.Neighboring,
                    new()
                    {
                        ModEntry.Instance.Api.MakeMRetain(),
                    }
                ),
                ModEntry.Instance.Api.MakeAModifierWrapper
                (
                    IPhilipAPI.CardModifierTarget.Neighboring,
                    new()
                    {
                        ModEntry.Instance.Api.MakeMUnplayable(),
                    }
                )
            };
        }

        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                new() { ModEntry.Instance.Api.MakeMRetain() }
            )
        };
    }
}


internal sealed class JettisonParts : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Last_Resort"].Sprite;
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

internal sealed class StunMod : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Stun_Mod"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            flippable = true,
            unplayable = upgrade != Upgrade.B
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<ICardModifier> mods = new() { ModEntry.Instance.Api.MakeMStun() };
        if (upgrade == Upgrade.A) mods.Add(ModEntry.Instance.Api.MakeMAddAction(new AStatus() { status = Status.stunCharge, targetPlayer = true, statusAmount = 1 }, ModEntry.Instance.sprites["icon_sticker_stun"].Sprite));

        List<CardAction> actions = new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                mods
            ),
            new ADrawCard() { count = 1 },
        };
        if (upgrade == Upgrade.B) actions.Add(new AAttack() { damage = GetDmg(s, 0), stunEnemy = true });

        return actions;
    }
}

internal sealed class Oops : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Oops"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            unplayable = true,
            retain = upgrade == Upgrade.B
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                upgrade == Upgrade.A ? IPhilipAPI.CardModifierTarget.Directional_WholeHand : IPhilipAPI.CardModifierTarget.Neighboring,
                new()
                {
                    ModEntry.Instance.Api.MakeMBuffAttack(upgrade == Upgrade.A ? -2 : -1),
                    ModEntry.Instance.Api.MakeMAddAction
                    (
                        new AStatus() {
                            status = ModEntry.Instance.RedrawStatus.Status,
                            targetPlayer = true,
                            statusAmount = upgrade == Upgrade.A ? 2 : 1,
                        },
                        ModEntry.Instance.sprites["icon_sticker_redraw"].Sprite
                    )
                }
            )
        };
    }
}

internal sealed class ReduceReuse : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Recycle_Parts"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            flippable = true,
            unplayable = upgrade == Upgrade.B
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        if (upgrade == Upgrade.B)
        {
            return new()
            {
                ModEntry.Instance.Api.MakeAModifierWrapper
                (
                    IPhilipAPI.CardModifierTarget.Directional,
                    new()
                    {
                        ModEntry.Instance.Api.MakeMRecycle(),
                        ModEntry.Instance.Api.MakeMDeleteActions(),
                        ModEntry.Instance.Api.MakeMSetEnergyCostToZero(),
                    }
                ),
                ModEntry.Instance.Api.MakeAModifierWrapper
                (
                    IPhilipAPI.CardModifierTarget.Directional,
                    new()
                    {
                        ModEntry.Instance.Api.MakeMMakePlayable(),
                        ModEntry.Instance.Api.MakeMDontExhaust(),
                    }
                )
            };
        }

        List<CardAction> actions = new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional,
                new() { ModEntry.Instance.Api.MakeMRecycle() }
            ),
            new ADrawCard() { count = 1 },
        };

        if (upgrade == Upgrade.A) actions.Add(new AStatus() { status = ModEntry.Instance.RedrawStatus.Status, targetPlayer = true, statusAmount = 2 });

        return actions;
    }
}

internal sealed class OpenBayDoors : Card, IRegisterableCard
{
    public static Rarity GetRarity() => Rarity.common;
    public static Spr GetArt() => ModEntry.Instance.sprites["card_Open_Bay_Doors"].Sprite;

    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            retain = upgrade == Upgrade.A
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<ICardModifier> modifiers = new()
        {
            ModEntry.Instance.Api.MakeMExhaust(),
            ModEntry.Instance.Api.MakeMReduceEnergyCost()
        };

        if (upgrade == Upgrade.B)
        {
            modifiers.Add
            (
                ModEntry.Instance.Api.MakeMAddAction
                (
                    new ASpawn() { thing = new Missile() { missileType = MissileType.normal } },
                    ModEntry.Instance.sprites["icon_sticker_missile"].Sprite
                )
            );
        }

        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional_WholeHand,
                modifiers,
                new()
                {
                    direction = upgrade == Upgrade.A ? IPhilipAPI.CardModifierTargetDirection.RIGHT : IPhilipAPI.CardModifierTargetDirection.LEFT
                }
            )
        };
    }
}
