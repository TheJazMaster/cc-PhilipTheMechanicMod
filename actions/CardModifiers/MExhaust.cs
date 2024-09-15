using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MExhaust : BasicCardModifier, ICardDataModifier
{
    public bool value = true;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites[value ? "icon_sticker_exhaust" : "icon_sticker_dont_exhaust"];

    public override Icon? GetIcon() => new Icon(value ? StableSpr.icons_exhaust : ModEntry.Instance.sprites["icon_dont_exhaust"], null, Colors.textMain);

	public override double Priority => value ? Priorities.MODIFY_DATA_UNFAVORABLE : Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        data.exhaust = value;
        return data;
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        return [
            new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.actionMisc,
                () => GetIcon()!.Value!.path,
                () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, value.ToString(), "name"]),
                () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, value.ToString(), "description"]),
                key: GetType().FullName ?? GetType().Name
            )
        ];
    }
}