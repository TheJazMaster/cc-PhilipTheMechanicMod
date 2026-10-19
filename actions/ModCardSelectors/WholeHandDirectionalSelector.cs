using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

public sealed class WholeHandDirectionalSelector : DirectionalSelector
{
	public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
    {
        return left
            ? affectingIndex < originIndex
            : affectingIndex > originIndex;
    }

    
    private Spr GetSprite(bool isFlimsy, bool overwrites)
    {
        if (isFlimsy)
        {
            if (overwrites) {
                return left
                    ? ModEntry.Instance.sprites["icon_FlimsyOverwrite_All_Left_Card_Mod"]
                    : ModEntry.Instance.sprites["icon_FlimsyOverwrite_All_Right_Card_Mod"];
            }
            return left
                ? ModEntry.Instance.sprites["icon_Flimsy_All_Left_Card_Mod"]
                : ModEntry.Instance.sprites["icon_Flimsy_All_Right_Card_Mod"];
        }
        if (overwrites) {
            return left
                ? ModEntry.Instance.sprites["icon_Overwrite_All_Left_Card_Mod"]
                : ModEntry.Instance.sprites["icon_Overwrite_All_Right_Card_Mod"];
        }
        else
        {
            return left
                ? ModEntry.Instance.sprites["icon_all_cards_to_the_left"]
                : ModEntry.Instance.sprites["icon_all_cards_to_the_right"];
        }
    }


    public override Icon? GetIcon(State s, bool isFlimsy, bool overwrites) => new Icon(GetSprite(isFlimsy, overwrites), null, Colors.textMain);

    public override List<Tooltip> GetTooltips(State s, bool isFlimsy, bool overwrites)
    {
        Spr sprite = GetSprite(isFlimsy, overwrites);   
        List<Tooltip> ret = [
            new GlossaryTooltip($"modifier.{GetType().Namespace!}::{GetType().Name}") {
                TitleColor = Colors.action,
                Icon = sprite,
                Title = ModEntry.Instance.Localizations.Localize(["action", "AWholeHandDirectionalMod", "name", left ? "left" : "right"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "AWholeHandDirectionalMod", "description", left ? "left" : "right"]),
            }
        ];
        return AddAdditionalTooltips(ret, sprite, isFlimsy, overwrites, "  ");
    }
}