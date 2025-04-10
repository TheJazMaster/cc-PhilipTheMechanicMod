using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

public sealed class WholeHandSelector : ModCardSelector
{
	public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
    {
        return affectingIndex != originIndex;
    }

    private static Spr GetSprite(bool isFlimsy, bool overwrites)
    {
        if (isFlimsy) {
            if (overwrites) {
                return ModEntry.Instance.sprites["icon_FlimsyOverwrite_All_Other_Cards_Mod"];
            }
            return ModEntry.Instance.sprites["icon_Flimsy_All_Other_Cards_Mod"];
        }
        if (overwrites) return ModEntry.Instance.sprites["icon_Overwrite_All_Other_Cards_Mod"];
        else return ModEntry.Instance.sprites["icon_card_all_other_cards"];
    }

    public override Icon? GetIcon(State s, bool isFlimsy, bool overwrites) => new Icon(GetSprite(isFlimsy, overwrites), null, Colors.textMain);
    
    public override List<Tooltip> GetTooltips(State s, bool isFlimsy, bool overwrites)
    {
        Spr sprite = GetSprite(isFlimsy, overwrites);
        List<Tooltip> ret = [
            new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
                TitleColor = Colors.action,
                Icon = sprite,
                Title = ModEntry.Instance.Localizations.Localize(["action", "AWholeHandMod", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "AWholeHandMod", "description"]),
            }
        ];
        return AddAdditionalTooltips(ret, sprite, isFlimsy, overwrites, "     ");
    }
}