using clay.PhilipTheMechanic.Controllers;
using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MDontExhaust : BasicCardModifier, ICardDataModifier
{
    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_dont_exhaust"];
    
    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_dont_exhaust"], null, Colors.textMain);

	public override double Priority => Priorities.MODIFY_DATA_FAVORABLE;

    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
    {
        data.exhaust = false;
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