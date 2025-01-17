using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

public class AWholeHandDirectionalCardsModifierWrapper : ADirectionalCardModifierWrapper
{
	public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
    {
        return left
            ? affectingIndex < originIndex
            : affectingIndex > originIndex;
    }

    public override Icon? GetIcon(State s)
    {
        if (isFlimsy)
        {
            return left
                ? new Icon(ModEntry.Instance.sprites["icon_Flimsy_All_Left_Card_Mod"], null, Colors.textMain)
                : new Icon(ModEntry.Instance.sprites["icon_Flimsy_All_Right_Card_Mod"], null, Colors.textMain);
        }
        else
        {
            return left
                ? new Icon(ModEntry.Instance.sprites["icon_all_cards_to_the_left"], null, Colors.textMain)
                : new Icon(ModEntry.Instance.sprites["icon_all_cards_to_the_right"], null, Colors.textMain);
        }
    }

    public override Tooltip? GetTooltip(State s)
    {
        List<Tooltip> tooltips = [];

        if (isFlimsy)
        {
            return new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.sprites[left ? "icon_Flimsy_All_Left_Card_Mod" : "icon_Flimsy_All_Right_Card_Mod"],
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandDirectionalMod_Flimsy", "name", left ? "left" : "right"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandDirectionalMod_Flimsy", "description", left ? "left" : "right"]),
                key: (typeof(AWholeHandDirectionalCardsModifierWrapper).FullName ?? typeof(AWholeHandDirectionalCardsModifierWrapper).Name) + "Flimsy"
            );
        }
        else
        {
            return new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.sprites[left ? "icon_all_cards_to_the_left" : "icon_all_cards_to_the_right"],
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandDirectionalMod", "name", left ? "left" : "right"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandDirectionalMod", "description", left ? "left" : "right"]),
                key: typeof(AWholeHandDirectionalCardsModifierWrapper).FullName ?? typeof(AWholeHandDirectionalCardsModifierWrapper).Name
            );
        }
    }
}