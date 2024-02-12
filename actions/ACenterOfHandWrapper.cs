using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    public class ACenterOfHandWrapper : CardAction
    {
        public bool isCenter = true;
        public required List<CardAction> actions;

        public override void Begin(G g, State s, Combat c)
        {
            
        }
    }
}
