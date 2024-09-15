using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;
    
public class MAddAction : CardModifier, ICardActionModifier
{
    public bool goLast = false;
    public static readonly string IsModActionKey = "IsModAction";
    public required CardAction action;
    public Spr? stickerSprite;

    public override bool RequestsStickyNote() => true;

	public override double Priority => goLast ? Priorities.ADD_ACTION_LAST : Priorities.ADD_ACTION;

    public override Spr? GetSticker(State s) => stickerSprite;

    public override CardAction GetActionForRendering(State s) => action;

    public override List<Tooltip> GetTooltips(State s) => action.GetTooltips(s); 

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
    {
        actions.Add(Mutil.DeepCopy(action));
        return actions;
    }
}