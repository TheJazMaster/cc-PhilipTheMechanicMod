﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MMakePlayable : ICardModifier
    {
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_card_playable"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(ModEntry.Instance.sprites["icon_card_playable"].Sprite, null, Colors.textMain);
        }
        public CardData TransformData(CardData data, State s, Combat c)
        {
            data.unplayable = false;
            return data;
        }
    }
}
