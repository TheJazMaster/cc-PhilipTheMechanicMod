using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    public class AAddCardUpgraded : AAddCard
    {
        public override Icon? GetIcon(State s)
        {
            return new Icon(ModEntry.Instance.sprites["icon_addUpgradedCard"], base.amount, Colors.textBold);
        }
    }
}
