using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.actions
{
    public class AAddCardNoIcon : AAddCard
    {
        public override Icon? GetIcon(State s)
        {
            return null;
        }
    }
}
