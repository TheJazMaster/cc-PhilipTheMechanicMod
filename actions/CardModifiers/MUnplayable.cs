using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MPlayable : BasicCardModifier, ICardDataModifier
{
    public bool value = true;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites[value ? "icon_sticker_card_playable" : "icon_sticker_card_unplayable"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites[value ? "icon_card_playable" : "icon_card_unplayable"], null, Colors.textMain);

	public override double Priority => value ? Priorities.MODIFY_DATA_FAVORABLE : Priorities.MODIFY_DATA_UNFAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        data.unplayable = !value;
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