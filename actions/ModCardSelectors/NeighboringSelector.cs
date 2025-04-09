using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

public sealed class NeighboringSelector : ModCardSelector
{
    public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
    {
        for (int i = 1; i <= range; i++) {
            if (affectingIndex == originIndex - i || affectingIndex == originIndex + i) return true;
        }
        return false;
    }

    public override Icon? GetIcon(State s, bool isFlimsy, bool overwrites)
    {
        if (isFlimsy) {
            if (overwrites) {
                return new Icon(ModEntry.Instance.sprites["icon_FlimsyOverwrite_Neighbors_Card_Mod"], null, Colors.textMain);
            }
            return new Icon(ModEntry.Instance.sprites["icon_Flimsy_Neighbors_Card_Mod"], null, Colors.textMain);
        }
        if (overwrites) {
            return new Icon(ModEntry.Instance.sprites["icon_Overwrite_Neighbors_Card_Mod"], null, Colors.textMain);
        }
        else {
            return new Icon(ModEntry.Instance.sprites["icon_card_neighbors"], null, Colors.textMain);
        }
    }


    public override List<Tooltip> GetTooltips(State s, bool isFlimsy, bool overwrites)
    {
        Spr sprite;
        if (isFlimsy) {
            if (overwrites) sprite = ModEntry.Instance.sprites["icon_FlimsyOverwrite_Neighbors_Card_Mod"];
            else sprite = ModEntry.Instance.sprites["icon_Flimsy_Neighbors_Card_Mod"];
        } else {
            if (overwrites) sprite = ModEntry.Instance.sprites["icon_Overwrites_Neighbors_Card_Mod"];
            else sprite = ModEntry.Instance.sprites["icon_card_neighbors"];
        }
        
        List<Tooltip> ret = [
            new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {// + "FlimsyOverwrite"
                TitleColor = Colors.action,
                Icon = sprite,
                Title = ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod", "description"]),
            }
        ];
        return AddAdditionalTooltips(ret, sprite, isFlimsy, overwrites);
    }
}