using Shockah;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions
{
    public class ASingleDirectionalCardModifierWrapper : ADirectionalCardModifierWrapper
    {
		public override bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
        {
            int offset = left ? -1 : 1;
            for (int i = 1; i <= range; i++) {
                if (affectingIndex == originIndex + offset*i) return true;
            }
            return false;
        }

        public override Icon? GetIcon(State s)
        {
            if (isFlimsy)
            {
                return left
                    ? new Icon(ModEntry.Instance.sprites["icon_Flimsy_Left_Card_Mod"], null, Colors.textMain)
                    : new Icon(ModEntry.Instance.sprites["icon_Flimsy_Right_Card_Mod"], null, Colors.textMain);
            }
            else
            {
                return left
                    ? new Icon(ModEntry.Instance.sprites["icon_card_to_the_left"], null, Colors.textMain)
                    : new Icon(ModEntry.Instance.sprites["icon_card_to_the_right"], null, Colors.textMain);
            }
        }

        public override Tooltip? GetTooltip(State s)
        {
            List<Tooltip> tooltips = [];

            if (isFlimsy)
            {
                return new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites[left ? "icon_Flimsy_Left_Card_Mod" : "icon_Flimsy_Right_Card_Mod"],
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod_Flimsy", "name", left ? "left" : "right"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod_Flimsy", "description", left ? "left" : "right"]),
                    key: typeof(ADirectionalCardModifierWrapper).FullName ?? typeof(ADirectionalCardModifierWrapper).Name
                );
            }
            else
            {
                return new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites[left ? "icon_card_to_the_left" : "icon_card_to_the_right"],
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod", "name", left ? "left" : "right"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod", "description", left ? "left" : "right"]),
                    key: typeof(ADirectionalCardModifierWrapper).FullName ?? typeof(ADirectionalCardModifierWrapper).Name
                );
            }
        }
    }
}
