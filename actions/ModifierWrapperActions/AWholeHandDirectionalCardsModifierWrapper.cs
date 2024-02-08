using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions
{
    public class AWholeHandDirectionalCardsModifierWrapper : AModifierWrapper
    {
        public bool left;
        
        public override bool IsTargeting(Card potetialTarget, Card myOwnerCard, Combat c)
        {
            int selfIndex = c.hand.IndexOf(myOwnerCard);
            int queryIndex = c.hand.IndexOf(potetialTarget);

            // should never happen, but just in case
            if (queryIndex == -1 || selfIndex == -1) { return false; }

            return left
                ? queryIndex < selfIndex
                : queryIndex > selfIndex;
        }

        public override Icon? GetIcon(State s)
        {
            if (isFlimsy)
            {
                return left
                    ? new Icon(ModEntry.Instance.sprites["icon_Flimsy_All_Left_Card_Mod"].Sprite, null, Colors.textMain)
                    : new Icon(ModEntry.Instance.sprites["icon_Flimsy_All_Right_Card_Mod"].Sprite, null, Colors.textMain);
            }
            else
            {
                return left
                    ? new Icon(ModEntry.Instance.sprites["icon_all_cards_to_the_left"].Sprite, null, Colors.textMain)
                    : new Icon(ModEntry.Instance.sprites["icon_all_cards_to_the_right"].Sprite, null, Colors.textMain);
            }
        }
    }
}
