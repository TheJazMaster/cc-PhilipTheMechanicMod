using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MPlayTwice : BasicCardModifier, ICardActionModifier
{
    public override bool RequestsStickyNote() => false;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_2x_sticker"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_play_twice"], null, Colors.textMain);

	public override double Priority => Priorities.MANIPULATE_ACTIONS;

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
    {
        if (isRendering) {
            actions.Add(new APlayTwiceDummy());
            return actions;
        }

        actions.AddRange(Mutil.DeepCopy(actions));
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