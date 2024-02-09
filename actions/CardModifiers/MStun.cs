using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MStun : ICardModifier
    {
        public int amount;
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_stun"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_stun"), amount, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c)
        {
            foreach (var action in actions)
            {
                if (action is AAttack aattack)
                {
                    aattack.stunEnemy = true;
                }
            }
            return actions;
        }
    }
}
