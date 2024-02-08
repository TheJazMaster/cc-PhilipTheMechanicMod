using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MRetain : ICardModifier
    {
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_retain"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_retain"), null, Colors.textMain);
        }
        public CardData TransformData(CardData data, State s, Combat c)
        {
            data.retain = true;
            return data;
        }
    }
}
