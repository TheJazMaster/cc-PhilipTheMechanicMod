using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MShieldForAttackAmount : BasicCardModifier, ICardActionModifier
{
    public bool tempShield = true;

    public override bool RequestsStickyNote() 
    {
        return true;
    }

    public override Spr? GetSticker(State s) => tempShield 
            ? ModEntry.Instance.sprites["icon_sticker_temp_shield"] 
            : ModEntry.Instance.sprites["icon_sticker_shield"];

    public override Icon? GetIcon() => tempShield
            ? new Icon(ModEntry.Instance.sprites["icon_temp_shield_x"], null, Colors.textMain)
            : new Icon(ModEntry.Instance.sprites["icon_shield_x"], null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_ACTIONS;

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        success = false;
        int amount = 0;
        foreach (CardAction action in actions)
        {
            var actionsLevel2 = ModEntry.Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(action, true);
            foreach (var action2 in actionsLevel2) if (action2 is AAttack a) {
                amount += a.damage;
                success = true;
            }
        }

        if (success) {
            actions.Add(new AStatus() 
            {
                status = tempShield ? Status.tempShield : Status.shield,
                targetPlayer = true,
                statusAmount = amount,
                xHint = 1
            });
        }

        return actions;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name", tempShield ? "temp" : "regular"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description", tempShield ? "temp" : "regular"]),
        }
    ];
}