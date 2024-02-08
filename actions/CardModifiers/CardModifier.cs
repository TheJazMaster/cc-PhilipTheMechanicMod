using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class CardModifier
    {
        public virtual Spr? GetSticker(State s) { return null; }
        public virtual Icon? GetIcon(State s) { return null; }
        public virtual List<Tooltip> GetTooltips(State s) { return new(); }
        public virtual List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c) { return actions; }
        public virtual CardData TransformData(CardData data, State s, Combat c) { return data; }
    }
}
