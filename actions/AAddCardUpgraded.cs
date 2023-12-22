using CobaltCoreModding.Definitions.ExternalItems;
using FSPRO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic.actions
{
    public class AAddCardUpgraded : AAddCard
    {
        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)MainManifest.sprites["icon_addUpgradedCard"].Id, null, Colors.heal);
        }
    }
}
