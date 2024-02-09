using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MShieldForAttackAmount : ICardModifier
    {
        public bool tempShield = true;
        public bool RequestsStickyNote() 
        {
            return true;
        }

        public Spr? GetSticker(State s)
        {
            return tempShield 
                ? ModEntry.Instance.sprites["icon_sticker_shield"].Sprite 
                : ModEntry.Instance.sprites["icon_sticker_temp_shield"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return tempShield
                ? new Icon(ModEntry.Instance.sprites["icon_temp_shield_x"].Sprite, null, Colors.textMain)
                : new Icon(ModEntry.Instance.sprites["icon_shield_x"].Sprite, null, Colors.textMain);
        }
        List<Tooltip> GetTooltips(State s) 
        {
            return new(); // TODO 
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c)
        {
            int amount = 0;
            foreach (CardAction action in actions)
            {
                // TODO: kokoro unwrap
                if (action is AAttack a) amount += a.damage;
            }

            actions.Add(new AStatus() 
            {
                status = tempShield ? Status.tempShield : Status.shield,
                targetPlayer = true,
                statusAmount = amount,
            });

            return actions;
        }
    }
}
