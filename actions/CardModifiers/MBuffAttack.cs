using System;
using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MBuffAttack : BasicCardModifier, ICardActionModifier
{
    public int amount;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_buff_attack"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_attack_buff"], amount, Colors.textMain);

	public override double Priority => Priorities.MODIFY_ACTIONS;
    
    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        success = false;
        foreach (var action in actions)
        {
            var actionsLevel2 = ModEntry.Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(action, true);
            foreach (var action2 in actionsLevel2) success |= ModifyAction(action2);
        }

        return actions;
    }
    private bool ModifyAction(CardAction action)
    {
        if (action is AAttack aattack)
        {
            aattack.damage = Math.Max(0, aattack.damage + amount);
            return true;
        }
        return false;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"], new { Amount = amount }),
        }
    ];
}