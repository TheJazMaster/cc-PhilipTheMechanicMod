using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MAddAction : ICardModifier
    {
        public required CardAction action;
        public Spr? stickerSprite;

        public bool RequestsStickyNote() 
        {
            return true;
        }

        public Spr? GetSticker(State s)
        {
            return stickerSprite;
        }
        public Icon? GetIcon(State s)
        {
            return action.GetIcon(s);
        }
        List<Tooltip> GetTooltips(State s) 
        {
            return action.GetTooltips(s); 
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
        {
            actions.Add(Mutil.DeepCopy(action));
            return actions;
        }
    }
}
