using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MAttacksPierce : ICardModifier
    {
        public int amount;
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_piercing"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_attackPiercing"), amount, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c)
        {
            // TODO: kokoro unwrap
            foreach (var action in actions)
            {
                if (action is AAttack aattack)
                {
                    aattack.piercing = true;
                }
            }
            return actions;
        }
    }
}
