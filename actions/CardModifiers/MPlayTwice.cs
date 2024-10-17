using Microsoft.Extensions.Logging;
using Shockah;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MPlayTwice : BasicCardModifier, ICardActionModifier
{
    private bool ignoreDouble = false;
    
    public override bool RequestsStickyNote() => false;

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites["icon_2x_sticker"];

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites["icon_play_twice"], null, Colors.textMain);

	public override double Priority => Priorities.MANIPULATE_ACTIONS;

    public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
    {
        if (ignoreDouble) return actions;
        if (isRendering) {
            actions.Add(new APlayTwiceDummy());
            return actions;
        }

        ignoreDouble = true;
        var newActions = card.GetActionsOverridden(s, c);
        ModEntry.Instance.Logger.LogInformation("aaaaa " + newActions.Count + " " + actions.Count);
        ModEntry.Instance.Logger.LogInformation("NEW");
        foreach(CardAction na in newActions) {
            ModEntry.Instance.Logger.LogInformation(na.Key());
        }
        ModEntry.Instance.Logger.LogInformation("OLD");
        foreach(CardAction aa in actions) {
            ModEntry.Instance.Logger.LogInformation(aa.Key());
        }
        if (newActions.Count > 0)
            actions.AddRange(newActions[0..Math.Min(newActions.Count, actions.Count)]);
        ignoreDouble = false;
        return actions;
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