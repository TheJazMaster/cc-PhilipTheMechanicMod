using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.actions
{
    public class ATooltipDummy : ADummyAction
    {
        public List<Tooltip> tooltips;

        public override List<Tooltip> GetTooltips(State s)
        {
            return tooltips;
        }
    }
}
