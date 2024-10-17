using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MInfinite : BasicCardModifier, ICardDataModifier
{
    public bool value = true;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites[value ? "icon_sticker_infinite" : "icon_sticker_no_infinite"];

    public override Icon? GetIcon() => new Icon(value ? StableSpr.icons_infinite : ModEntry.Instance.sprites["icon_no_infinite"], null, Colors.textMain);

	public override double Priority => value ? Priorities.MODIFY_DATA_FAVORABLE : Priorities.MODIFY_DATA_UNFAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        data.infinite = value;
        return data;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.actionMisc,
            () => GetIcon()!.Value!.path,
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, value.ToString(), "name"]),
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, value.ToString(), "description"]),
            key: GetType().FullName ?? GetType().Name
        ),
        new TTGlossary("cardtrait.infinite")
    ];
}