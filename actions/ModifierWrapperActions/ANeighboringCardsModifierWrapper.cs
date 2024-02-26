using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions
{
    public class ANeighboringCardsModifierWrapper : AModifierWrapper
    {   
        public override bool IsTargeting(Card potetialTarget, Card myOwnerCard, Combat c)
        {
            int selfIndex = c.hand.IndexOf(myOwnerCard);
            int queryIndex = c.hand.IndexOf(potetialTarget);

            // should never happen, but just in case
            if (queryIndex == -1 || selfIndex == -1) { return false; }

            return queryIndex == selfIndex - 1 || queryIndex == selfIndex + 1;
        }

        public override Icon? GetIcon(State s)
        {
            if (isFlimsy)
            {
                return new Icon(ModEntry.Instance.sprites["icon_Flimsy_Neighbors_Card_Mod"].Sprite, null, Colors.textMain);
            }
            else
            {
                return new Icon(ModEntry.Instance.sprites["icon_card_neighbors"].Sprite, null, Colors.textMain);
            }
        }


        public override Tooltip? GetTooltip(State s)
        {
            List<Tooltip> tooltips = new();

            if (isFlimsy)
            {
                return new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites["icon_Flimsy_Neighbors_Card_Mod"].Sprite,
                    () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod_Flimsy", "name"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod_Flimsy", "description"]),
                    key: typeof(ANeighboringCardsModifierWrapper).FullName ?? typeof(ANeighboringCardsModifierWrapper).Name
                );
            }
            else
            {
                return new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => ModEntry.Instance.sprites["icon_card_neighbors"].Sprite,
                    () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod", "name"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "ANeighborsMod", "description"]),
                    key: typeof(ANeighboringCardsModifierWrapper).FullName ?? typeof(ANeighboringCardsModifierWrapper).Name
                );
            }
        }
    }
}
