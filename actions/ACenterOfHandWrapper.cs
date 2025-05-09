﻿using Microsoft.Extensions.Logging;
using Nickel;
using System.Collections.Generic;
using System.Linq;

namespace clay.PhilipTheMechanic.Actions;

public class ACenterOfHandWrapper : AMultiIconAction
{
    public bool isCenter = true;
    public required List<CardAction> actions;

    private ACenterOfHandWrapper() {}
    public static ACenterOfHandWrapper Make(bool isCenter, List<CardAction> actions, bool disabled) {
        if (disabled && actions != null) {
            foreach (CardAction action in actions) {
                action.disabled = true;
            }
        }
        return new ACenterOfHandWrapper {
            actions = actions ?? [],
            isCenter = isCenter,
            disabled = disabled
        };
    }

    public override void Begin(G g, State s, Combat c)
    {
        if (disabled) return;
        c.QueueImmediate(actions);
    }

    public override Icon? GetIcon(State s) =>
        new Icon(ModEntry.Instance.sprites[isCenter ? "icon_card_is_centered": "icon_card_is_not_centered"], null, Colors.textMain);

    public override List<CardAction> GetActionsForRendering(State s) 
    { 
        List<CardAction> ret = [];
        var ownIcon = GetIcon(s);
        if (ownIcon.HasValue) {
            ret.Add(new AIconDummy {
                icon = ownIcon.Value
            });
        }
        ret.AddRange(actions);
        return ret;
    }

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}{isCenter}") {
            TitleColor = Colors.action,
            Icon = isCenter
                ? ModEntry.Instance.sprites["icon_card_is_centered"]
                : ModEntry.Instance.sprites["icon_card_is_not_centered"],
            Title = ModEntry.Instance.Localizations.Localize(["condition", isCenter ? "CCardCentered" : "CCardNotCentered", "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["condition", isCenter ? "CCardCentered" : "CCardNotCentered", "description"]),
        },
        .. actions.SelectMany(a => a.GetTooltips(s)).ToList()
        
    ];
}