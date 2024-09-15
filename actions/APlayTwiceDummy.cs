using HarmonyLib;
using Shockah;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions;

public class APlayTwiceDummy : ADummyAction
{
        public override List<Tooltip> GetTooltips(State s)
        {
            return [
                new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites["icon_play_twice"],
                    () => ModEntry.Instance.Localizations.Localize(["action", "APlayTwiceDummy", "name"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "APlayTwiceDummy", "description"]),
                    key: typeof(ANanobots).FullName ?? typeof(ANanobots).Name
                )
            ];
        }

    public override Icon? GetIcon(State s) => new Icon(ModEntry.Instance.sprites["icon_play_twice"], null, Colors.textMain);
}
