using System.Collections.Generic;
using Shockah;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MExtendModifiers : BasicCardModifier
{
    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_extend"], null, Colors.textMain);

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_extend"];

	public override double Priority => Priorities.MODIFY_ACTIONS;

    public override List<Tooltip> GetTooltips(State s) => [
        new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.actionMisc,
            () => GetIcon()!.Value!.path,
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
            key: GetType().FullName ?? GetType().Name
        )
    ];
}
