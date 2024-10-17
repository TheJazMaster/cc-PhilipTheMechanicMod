using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MRetain : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_retain"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_retain, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        data.retain = true;
        return data;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.actionMisc,
            () => GetIcon()!.Value!.path,
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
            key: GetType().FullName ?? GetType().Name
        ),
        new TTGlossary("cardtrait.retain")
    ];
}