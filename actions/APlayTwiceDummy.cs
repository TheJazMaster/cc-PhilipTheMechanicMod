using HarmonyLib;
using Nickel;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions;

public class APlayTwiceDummy : ADummyAction
{
        public override List<Tooltip> GetTooltips(State s)
        {
            return [
                new GlossaryTooltip($"action.{GetType().Namespace!}::{GetType().Name}") {
                    TitleColor = Colors.action,
                    Icon = ModEntry.Instance.sprites["icon_play_twice"],
                    Title = ModEntry.Instance.Localizations.Localize(["action", "APlayTwiceDummy", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["action", "APlayTwiceDummy", "description"]),
                }
            ];
        }

    public override Icon? GetIcon(State s) => new Icon(ModEntry.Instance.sprites["icon_play_twice"], null, Colors.textMain);
}
