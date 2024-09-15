using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MInfinite : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_infinite"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_infinite, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        data.infinite = true;
        return data;
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