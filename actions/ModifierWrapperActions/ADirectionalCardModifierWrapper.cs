using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions
{
    public class ADirectionalCardModifierWrapper : AModifierWrapper
    {
        public bool left;
        
        public override bool IsTargeting(Card potetialTarget, Card myOwnerCard, Combat c)
        {
            int selfIndex = c.hand.IndexOf(myOwnerCard);
            int queryIndex = c.hand.IndexOf(potetialTarget);

            // should never happen, but just in case
            if (queryIndex == -1 || selfIndex == -1) { return false; }

            int offset = left ? -1 : 1;
            return queryIndex == selfIndex + offset;
        }

        public override Icon? GetIcon(State s)
        {
            if (isFlimsy)
            {
                return left
                    ? new Icon(ModEntry.Instance.sprites["icon_Flimsy_Left_Card_Mod"].Sprite, null, Colors.textMain)
                    : new Icon(ModEntry.Instance.sprites["icon_Flimsy_Right_Card_Mod"].Sprite, null, Colors.textMain);
            }
            else
            {
                return left
                    ? new Icon(ModEntry.Instance.sprites["icon_card_to_the_left"].Sprite, null, Colors.textMain)
                    : new Icon(ModEntry.Instance.sprites["icon_card_to_the_right"].Sprite, null, Colors.textMain);
            }
        }

        public override Tooltip? GetTooltip(State s)
        {
            List<Tooltip> tooltips = new();

            if (isFlimsy)
            {
                return new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites[left ? "icon_Flimsy_Left_Card_Mod" : "icon_Flimsy_Right_Card_Mod"].Sprite,
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod_Flimsy", "name", left ? "left" : "right"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod_Flimsy", "description", left ? "left" : "right"]),
                    key: typeof(ADirectionalCardModifierWrapper).FullName ?? typeof(ADirectionalCardModifierWrapper).Name
                );
            }
            else
            {
                return new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites[left ? "icon_card_to_the_left" : "icon_card_to_the_right"].Sprite,
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod", "name", left ? "left" : "right"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "ADirectionalMod", "description", left ? "left" : "right"]),
                    key: typeof(ADirectionalCardModifierWrapper).FullName ?? typeof(ADirectionalCardModifierWrapper).Name
                );
            }
        }
    }
}
