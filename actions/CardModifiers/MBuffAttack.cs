using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MBuffAttack : CardModifier
    {
        public int amount;
        public override Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_buff_attack"].Sprite;
        }
        public override Icon? GetIcon(State s)
        {
            return new Icon(ModEntry.Instance.sprites["icon_attack_buff"].Sprite, amount, Colors.textMain);
        }
        public override List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c)
        {
            foreach (var action in actions)
            {
                if (action is AAttack aattack)
                {
                    aattack.damage = Math.Max(0, aattack.damage+amount);
                }
            }
            return actions;
        }
    }
}
