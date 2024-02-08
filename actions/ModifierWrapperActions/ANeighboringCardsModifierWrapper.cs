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
    }
}
