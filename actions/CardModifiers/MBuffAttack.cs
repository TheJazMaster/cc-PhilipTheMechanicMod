using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MBuffAttack : ICardModifier
    {
        public int amount;
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_buff_attack"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(ModEntry.Instance.sprites["icon_attack_buff"].Sprite, amount, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c)
        {

            foreach (var action in actions)
            {
                if (ModEntry.Instance.KokoroApi != null)
                {
                    var actionsLevel2 = ModEntry.Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(action, true);
                    foreach (var action2 in actionsLevel2) ModifyAction(action2);
                }
                else
                {
                    ModifyAction(action);
                }
            }

            return actions;
        }
        private void ModifyAction(CardAction action)
        {
            if (action is AAttack aattack)
            {
                aattack.damage = Math.Max(0, aattack.damage + amount);
            }
        }
    }
}
