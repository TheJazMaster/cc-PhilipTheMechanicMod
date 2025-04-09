using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MRetain : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_retain"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_retain, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;
    // public override bool IgnoresFlimsy => true;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        success = !data.retain;
        data.retain = true;
        return data;
    }

	// public override bool MattersForFlimsy() => false;
	

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
        },
        new TTGlossary("cardtrait.retain")
    ];
}