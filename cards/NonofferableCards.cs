using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using clay.PhilipTheMechanic.Controllers;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace clay.PhilipTheMechanic.Cards;

[HarmonyPatch]
internal sealed class Nanobots : Card
{

    public override List<CardAction> GetActions(State s, Combat c) 
    { 
        return [
			new ANanobots() { thisCardUuid = uuid },
            new ADummyAction(),
            new ADummyAction(),
        ]; 
    }

    public static void Replicate(State state, Combat combat)
    {
        var nanobotsInHand = combat.hand.Where(c => c is Nanobots).ToList();
        foreach(var nanobots in nanobotsInHand) 
        {
            combat.SendCardToHand(state, new Nanobots() { upgrade = nanobots.upgrade });
        }
    }

    public override CardData GetData(State state) => new()
    {
        retain = true,
        temporary = true,
        cost = upgrade == Upgrade.A ? 2 : 3,
        exhaust = upgrade == Upgrade.B

        //description = ModEntry.Instance.Localizations.Localize(["card", "Nanobots", "description", upgrade.ToString()])
    };
}

internal sealed class UraniumRound : Card
{
    public override CardData GetData(State state)
    {
        return new() {
            cost = upgrade == Upgrade.B ? 1 : 0,
            temporary = true,
            exhaust = upgrade == Upgrade.B
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return [
			new AAttack()
            {
                damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1),
                piercing = true,
                stunEnemy = true,
                status = upgrade == Upgrade.B ? Status.corrode : null,
                statusAmount = 1
            }
        ];
    }
}

internal sealed class ImpromptuBlastShield : ModifierCard
{
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            temporary = true,
            unplayable = true
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        List<CardModifier> modifiers = [
            ModEntry.Instance.Api.MakeMAddAction(
                new AStatus() { status = upgrade == Upgrade.B ? Status.shield : Status.tempShield, targetPlayer = true, statusAmount = 1 },
                ModEntry.Instance.sprites[upgrade == Upgrade.B ? "icon_sticker_shield" : "icon_sticker_temp_shield"]
            )
        ];
        return [
			upgrade == Upgrade.A ? new AWholeHandCardsModifierWrapper {
                modifiers = modifiers
            } :
            new AWholeHandDirectionalCardsModifierWrapper {
                modifiers = modifiers
            }
        ];
    }
}

internal sealed class OhNo : ModifierCard
{
    public override CardData GetData(State state)
    {
        return new() {
            cost = 0,
            retain = true,
            temporary = true,
            exhaust = true,
        };
    }

    public override List<AModifierWrapper> GetModifierActions(State s, Combat c)
    {
        return [
            new AWholeHandDirectionalCardsModifierWrapper {
                modifiers = [
                    new MDeleteActions(),
                    new MPlayable(),
                    new MExhaust()
                ]
            },
			new AWholeHandDirectionalCardsModifierWrapper {
                modifiers = [
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AStatus() { status = Status.evade, targetPlayer = true, statusAmount = upgrade == Upgrade.A ? 2 : 1 },
                        ModEntry.Instance.sprites["icon_sticker_evade"]
                    ),
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AStatus() { status = ModEntry.Instance.RedrawStatus, targetPlayer = true, statusAmount = upgrade == Upgrade.B ? 2 : 1 },
                        ModEntry.Instance.sprites["icon_sticker_redraw"]
                    ),
                ]
            },
        ];
    }
}

// Effect is hardcoded
internal sealed class ExtenderMod : ModifierCard
{
    public override CardData GetData(State state) => new() {
        cost = 1,
        unplayable = upgrade != Upgrade.A,
        floppable = true,
        recycle = upgrade == Upgrade.A,
        description = ModEntry.Instance.Localizations.Localize(["card", "ExtenderMod", "description", flipped ? "flipped" : "normal", upgrade.ToString()]),
        art = ModEntry.Instance.sprites[flipped ? "card_Extender_Mod_Flipped" : "card_Extender_Mod_Normal"]
    };

    public bool ExtendsMods() => !flipped;

    public bool SaveFlimsy() => upgrade == Upgrade.B && flipped;

	public override List<AModifierWrapper> GetModifierActions(State s, Combat c) => [
        new AWholeHandCardsModifierWrapper() {
            modifiers = [new MExtendModifiers()]
        }
    ];
}
