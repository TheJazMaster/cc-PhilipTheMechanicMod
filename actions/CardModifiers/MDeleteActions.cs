using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MDeleteActions : BasicCardModifier, ICardActionModifier, ICardDataModifier
{
    public override bool MandatesStickyNote() { return true; }

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_no_action"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_no_action"], null, Colors.textMain);

	public override double Priority => Priorities.REMOVE_ALL_ACTIONS;

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering) {
        return [];
    }
    
    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering) {
        data.description = null;
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