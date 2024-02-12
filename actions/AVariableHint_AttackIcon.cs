using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    internal class AVariableHint_AttackIcon : AVariableHint
    {
        public override Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_attack"), null, Colors.textMain);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            // TODO: return something like "total attack value on card"
            return new();
        }
    }
}
