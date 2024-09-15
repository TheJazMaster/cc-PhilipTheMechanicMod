using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MAttacksPierce : BasicCardModifier, ICardActionModifier
{

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_piercing"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_attackPiercing, null, Colors.textMain);

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

    private static void ModifyAction(CardAction action)
    {
        if (action is AAttack aattack)
        {
            aattack.piercing = true;
        }
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.actionMisc,
                () => GetIcon()!.Value!.path,
                () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
                () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
                key: GetType().FullName ?? GetType().Name
            )
        ];
    }
}