using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

public class ANeighboringCardsModifierWrapper : AModifierWrapper
{
    public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
    {
        for (int i = 1; i <= range; i++) {
            if (affectingIndex == originIndex - i || affectingIndex == originIndex + i) return true;
        }
        return false;
    }

    public override Icon? GetIcon(State s)
    {
        if (isFlimsy)
        {
            return new Icon(ModEntry.Instance.sprites["icon_Flimsy_Neighbors_Card_Mod"], null, Colors.textMain);
        }
        else
        {
            return new Icon(ModEntry.Instance.sprites["icon_card_neighbors"], null, Colors.textMain);
        }
    }


    public override Tooltip? GetTooltip(State s)
    {
        List<Tooltip> tooltips = new();

        if (isFlimsy)
        {
            return new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.sprites["icon_Flimsy_Neighbors_Card_Mod"],
                () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod_Flimsy", "name"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod_Flimsy", "description"]),
                key: (typeof(ANeighboringCardsModifierWrapper).FullName ?? typeof(ANeighboringCardsModifierWrapper).Name) + "Flimsy"
            );
        }
        else
        {
            return new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.sprites["icon_card_neighbors"],
                () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod", "name"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod", "description"]),
                key: typeof(ANeighboringCardsModifierWrapper).FullName ?? typeof(ANeighboringCardsModifierWrapper).Name
            );
        }
    }
}