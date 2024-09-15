using Shockah;
using System.Collections.Generic;

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

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
    {
        int amount = 0;
        foreach (CardAction action in actions)
        {
            var actionsLevel2 = ModEntry.Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(action, true);
            foreach (var action2 in actionsLevel2) if (action2 is AAttack a) amount += a.damage;
        }

        actions.Add(new AStatus() 
        {
            status = tempShield ? Status.tempShield : Status.shield,
            targetPlayer = true,
            statusAmount = amount,
            xHint = 1
        });

        return actions;
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.actionMisc,
                () => GetIcon()!.Value!.path,
                () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name", tempShield ? "temp" : "regular"]),
                () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description", tempShield ? "temp" : "regular"]),
                key: GetType().FullName ?? GetType().Name
            )
        ];
    }
}