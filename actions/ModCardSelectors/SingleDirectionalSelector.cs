using System.Collections.Generic;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

public sealed class SingleDirectionalSelector : DirectionalSelector
{
    public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
    {
        int offset = left ? -1 : 1;
        for (int i = 1; i <= range; i++) {
            if (affectingIndex == originIndex + offset*i) return true;
        }
        return false;
    }

    private Spr GetSprite(bool isFlimsy, bool overwrites)
    {
        if (isFlimsy)
        {
            if (overwrites) {
                return left
                    ? ModEntry.Instance.sprites["icon_FlimsyOverwrite_Left_Card_Mod"]
                    : ModEntry.Instance.sprites["icon_FlimsyOverwrite_Right_Card_Mod"];
            }
            return left
                ? ModEntry.Instance.sprites["icon_Flimsy_Left_Card_Mod"]
                : ModEntry.Instance.sprites["icon_Flimsy_Right_Card_Mod"];
        }
        if (overwrites) {
            return left
                ? ModEntry.Instance.sprites["icon_Overwrite_Left_Card_Mod"]
                : ModEntry.Instance.sprites["icon_Overwrite_Right_Card_Mod"];
        }
        else
        {
            return left
                ? ModEntry.Instance.sprites["icon_card_to_the_left"]
                : ModEntry.Instance.sprites["icon_card_to_the_right"];
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
                Title = ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod", "name", left ? "left" : "right"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod", "description", left ? "left" : "right"]),
            }
        ];
        return AddAdditionalTooltips(ret, sprite, isFlimsy, overwrites);
    }
}