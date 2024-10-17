using Shockah;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MBuffAttack : BasicCardModifier, ICardActionModifier
{
    public int amount;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_buff_attack"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_attack_buff"], amount, Colors.textMain);

	public override double Priority => Priorities.MODIFY_ACTIONS;
    
    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
    {
        foreach (var action in actions)
        {
            var actionsLevel2 = ModEntry.Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(action, true);
            foreach (var action2 in actionsLevel2) ModifyAction(action2);
        }

        return actions;
    }
    private void ModifyAction(CardAction action)
    {
        if (action is AAttack aattack)
        {
            aattack.damage = Math.Max(0, aattack.damage + amount);
        }
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.actionMisc,
            () => GetIcon()!.Value!.path,
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
            values: new List<Func<object>>() { () => amount },
            key: GetType().FullName ?? GetType().Name
        )
    ];
}