using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MStun : BasicCardModifier, ICardActionModifier
{

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_stun"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_stun, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_ACTIONS;

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
    {
        foreach (var action in actions)
        {
            if (action is AAttack aattack)
            {
                aattack.stunEnemy = true;
            }
        }
        return actions;
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