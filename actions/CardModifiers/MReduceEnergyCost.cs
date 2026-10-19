using Microsoft.Extensions.Logging;
using Nickel;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MReduceEnergyCost : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_energy_discount"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_discount, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        success = data.cost > 0;
        data.cost = Math.Max(0, data.cost - 1);
        return data;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
        }
    ];
}