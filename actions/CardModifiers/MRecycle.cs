using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MRecycle : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_recycle"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_recycle, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        success = !data.recycle;
        data.recycle = true;
        return data;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
        },
        new TTGlossary("cardtrait.recycle")
    ];
}