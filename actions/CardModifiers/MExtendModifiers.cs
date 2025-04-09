using System.Collections.Generic;
using System.Linq;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using clay.PhilipTheMechanic.Controllers;
using Microsoft.Extensions.Logging;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;

public class MExtendModifiers : BasicCardModifier, IModifierModifier
{
    public required bool? flimsyOverride;

    public override Icon? GetIcon() => new Icon(ModEntry.Instance.sprites[flimsyOverride == true ? "icon_extend_flimsy" : "icon_extend"], null, Colors.textMain);

    public override Spr? GetSticker(State s) => ModEntry.Instance.sprites[flimsyOverride == true ? "icon_sticker_extend_flimsy" : "icon_sticker_extend"];

    public void TransformModifiers(List<AModifierWrapper> wrappers, State s, Combat c, Card card, int? playedIndex, out bool success)
    {
        success = playedIndex.HasValue && ModifierCardsController.ModifierUsage[playedIndex.Value].Count > 0;
        foreach (AModifierWrapper wrapper in wrappers) {
            wrapper.selector = new WholeHandSelector();
            wrapper.isFlimsy = flimsyOverride ?? wrapper.isFlimsy;
        }
    }

	public override double Priority => Priorities.MODIFY_ACTIONS;

    public override List<Tooltip> GetTooltips(State s) => [
        new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
            TitleColor = Colors.keyword,
            Icon = GetIcon()!.Value!.path,
            Title = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
            Description = ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description", flimsyOverride?.ToString() ?? "None"]),
        }
    ];
}
