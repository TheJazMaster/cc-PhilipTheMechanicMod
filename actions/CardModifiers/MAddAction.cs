using System;
using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;
    
public class MAddAction : CardModifier, ICardActionModifier
{
    public bool goLast = false;
    public static readonly string IsModActionKey = "IsModAction";
    public override bool IgnoresFlimsy => false;
    public required CardAction action;
    public Spr? stickerSprite;

    public override bool RequestsStickyNote() => true;

	public override double Priority => goLast ? Priorities.ADD_ACTION_LAST : Priorities.ADD_ACTION;

    public override Spr? GetSticker(State s) => stickerSprite;

    public override CardAction GetActionForRendering(State s) => action;

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = action.GetIcon(s)!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
        },
        .. action.GetTooltips(s)
    ];

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering, out bool success)
    {
        if (!isRendering && action is AAddCard addCard) {
            addCard.card = addCard.card.CopyWithNewId();
        }
        actions.Add(Mutil.DeepCopy(action));
        success = true;
        return actions;
    }
}