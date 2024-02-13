using clay.PhilipTheMechanic.Actions;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Cards;

[HarmonyPatch]
internal sealed class Nanobots : Card
{
    public override List<CardAction> GetActions(State s, Combat c) 
    { 
        return new() 
        {
            new ANanobots() { thisCardUuid = this.uuid },
            new ADummyAction(),
        }; 
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
        cost = upgrade switch
        {
            Upgrade.None => 3,
            Upgrade.A => 2,
            Upgrade.B => 1,
        },
        infinite = upgrade == Upgrade.B, // yes this is worse unless you like expensive trash, that's intentional because you can only get this upgrade through weird trickery, like with Johnson
        //description = ModEntry.Instance.Localizations.Localize(["card", "Nanobots", "description", upgrade.ToString()])
    };
}

internal sealed class UraniumRound : Card
{
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 0,
            temporary = true,
            exhaust = upgrade != Upgrade.B,
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AAttack()
            {
                damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1),
                piercing = true,
                stunEnemy = true
            }
        };
    }
}

internal sealed class ImpromptuBlastShield : Card
{
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            temporary = true,
            unplayable = true,
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional_WholeHand,
                new()
                {
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AStatus() { status = upgrade == Upgrade.B ? Status.shield : Status.tempShield, targetPlayer = true, statusAmount = upgrade == Upgrade.A ? 3 : 1 },
                        ModEntry.Instance.sprites[upgrade == Upgrade.B ? "icon_sticker_shield" : "icon_sticker_temp_shield"].Sprite
                    )
                },
                new()
                {
                    isFlimsy = upgrade == Upgrade.A
                }
            )
        };
    }
}

internal sealed class OhNo : Card
{
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            retain = true,
            temporary = true,
            exhaust = true,
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional_WholeHand,
                new()
                {
                    ModEntry.Instance.Api.MakeMDeleteActions(),
                    ModEntry.Instance.Api.MakeMExhaust(),
                }
            ),
            ModEntry.Instance.Api.MakeAModifierWrapper
            (
                IPhilipAPI.CardModifierTarget.Directional_WholeHand,
                new()
                {
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AStatus() { status = Status.evade, targetPlayer = true, statusAmount = upgrade == Upgrade.A ? 2 : 1 },
                        ModEntry.Instance.sprites["icon_sticker_evade"].Sprite
                    ),
                    ModEntry.Instance.Api.MakeMAddAction(
                        new AStatus() { status = ModEntry.Instance.Api.RedrawStatus.Status, targetPlayer = true, statusAmount = upgrade == Upgrade.B ? 3 : 2 },
                        ModEntry.Instance.sprites["icon_sticker_redraw"].Sprite
                    ),
                }
            ),
        };
    }
}
