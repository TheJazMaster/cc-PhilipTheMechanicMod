using Microsoft.Extensions.Logging;
using Shockah;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MReduceEnergyCost : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_energy_discount"];

    public override Icon? GetIcon() => new Icon(StableSpr.icons_discount, null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        ModEntry.Instance.Logger.LogInformation(card.Name() + ": " + isRendering + ": " + data.cost);
        data.cost = Math.Max(0, data.cost - 1);
        return data;
    }

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