using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MStun : BasicCardModifier, ICardActionModifier
{

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_stun"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_stun, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_ACTIONS;

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        success = false;
        foreach (var action in actions)
        {
            if (action is AAttack aattack)
            {
                aattack.stunEnemy = true;
                success = true;
            }
        }
        return actions;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
        },
        new TTGlossary("action.stun")
    ];
}