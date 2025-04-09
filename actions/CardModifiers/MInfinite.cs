using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MInfinite : BasicCardModifier, ICardDataModifier
{
    public bool value = true;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites[value ? "icon_sticker_infinite" : "icon_sticker_no_infinite"];

    public override Icon? GetIcon() => new Icon(value ? StableSpr.icons_infinite : ModEntry.Instance.sprites["icon_no_infinite"], null, Colors.textMain);

	public override double Priority => value ? Priorities.MODIFY_DATA_FAVORABLE : Priorities.MODIFY_DATA_UNFAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        success = data.infinite != value;
        data.infinite = value;
        return data;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, value.ToString(), "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, value.ToString(), "description"]),
        },
        new TTGlossary("cardtrait.infinite")
    ];
}