using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MSetEnergyCostToZero : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_0_energy"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_0_energy"], null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        data.cost = 0;
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