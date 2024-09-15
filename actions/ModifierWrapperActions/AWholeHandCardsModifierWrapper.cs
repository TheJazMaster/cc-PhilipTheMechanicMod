using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

public class AWholeHandCardsModifierWrapper : AModifierWrapper
{
	public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
    {
        return affectingIndex != originIndex;
    }

    public override Icon? GetIcon(State s) =>
        isFlimsy ? new Icon(ModEntry.Instance.sprites["icon_Flimsy_All_Other_Cards_Mod"], null, Colors.textMain)
        : new Icon(ModEntry.Instance.sprites["icon_card_all_other_cards"], null, Colors.textMain);

    public override Tooltip? GetTooltip(State s)
    {
        List<Tooltip> tooltips = [];

        if (isFlimsy)
        {
            return new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.sprites["icon_Flimsy_All_Other_Cards_Mod"],
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandMod_Flimsy", "name"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandMod_Flimsy", "description"]),
                key: typeof(AWholeHandCardsModifierWrapper).FullName ?? typeof(AWholeHandCardsModifierWrapper).Name
            );
        }
        else
        {
            return new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.sprites["icon_card_all_other_cards"],
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandMod", "name"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "AWholeHandMod", "description"]),
                key: typeof(AWholeHandCardsModifierWrapper).FullName ?? typeof(AWholeHandCardsModifierWrapper).Name
            );
        }
    }
}