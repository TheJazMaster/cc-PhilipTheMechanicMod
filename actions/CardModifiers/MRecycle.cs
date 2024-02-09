using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MRecycle : ICardModifier
    {
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_recycle"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_recycle"), null, Colors.textMain);
        }
        public CardData TransformData(CardData data, State s, Combat c)
        {
            data.recycle = true;
            return data;
        }
    }
}
