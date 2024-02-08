using FSPRO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    public class AReplay : CardAction
    {
        // TODO: this
        //public override List<Tooltip> GetTooltips(State s)
        //{
        //    return new List<Tooltip>
        //    {
        //        new TTGlossary(ModEntry.Instance.glossary["AReplay"].Head, null)
        //    };
        //}

        public required List<CardAction> cardActions { get; set; }

        public override void Begin(G g, State s, Combat c)
        {
            foreach (CardAction item in cardActions)
            {
                if (item is AReplay) return;
                if (!(item is AEndTurn))
                {
                    c.Queue(item);
                }
            }
        }

        public override Icon? GetIcon(State s)
        {
            //return new Icon((Spr)MainManifest.sprites["icon_play_twice"].Id, null, Colors.heal);
            return null;
        }
    }
}
