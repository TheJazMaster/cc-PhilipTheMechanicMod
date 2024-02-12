using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MDeleteActions : ICardModifier
    {
        public bool MandatesStickyNote() { return true; }
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_no_action"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(ModEntry.Instance.sprites["icon_no_action"].Sprite, null, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card)
        {
            return new();
        }
    }
}
