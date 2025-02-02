﻿using System;
using System.Collections.Generic;
using Shockah;

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

    public override List<Tooltip> GetTooltips(State s) => [
        new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.actionMisc,
            () => action.GetIcon(s)!.Value!.path,
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
            key: GetType().FullName ?? GetType().Name
        ),
        .. action.GetTooltips(s)
    ];

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
    {
        if (!isRendering && action is AAddCard addCard) {
            addCard.card = addCard.card.CopyWithNewId();
        }
        actions.Add(Mutil.DeepCopy(action));
        return actions;
    }
}