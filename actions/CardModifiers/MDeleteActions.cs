using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using Microsoft.Extensions.Logging;
using Nickel;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MDeleteActions : BasicCardModifier, ICardActionModifier, ICardDataModifier, IModifierModifier
{
    public override bool MandatesStickyNote() { return true; }

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_sticker_no_action"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_no_action"], null, Colors.textMain);

	public override double Priority => Priorities.REMOVE_ALL_ACTIONS;

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering, out bool success) {
        success = actions.Count != 0;
        return [];
    }

    public void TransformModifiers(List<AModifierWrapper> wrappers, State s, Combat c, Card card, int? index, out bool success) {
        success = false;
        foreach (AModifierWrapper wrapper in wrappers) {
            wrapper.selector = new NoneSelector();
            success = true;
        }
    }
    
    public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering, out bool success) {
        success = data.unplayable;
        data.description = null;
        data.unplayable = false;
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