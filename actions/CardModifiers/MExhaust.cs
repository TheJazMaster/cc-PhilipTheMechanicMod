using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MExhaust : CardModifier
    {
        public override Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_exhaust"].Sprite;
        }
        public override Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_exhaust"), null, Colors.textMain);
        }
        public override CardData TransformData(CardData data, State s, Combat c)
        {
            data.exhaust = true;
            return data;
        }
    }
}
