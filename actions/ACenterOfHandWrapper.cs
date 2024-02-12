using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    public class ACenterOfHandWrapper : ATooltipDummy
    {
        public bool isCenter = true;
        public required List<CardAction> actions;

        public override void Begin(G g, State s, Combat c)
        {
            if (disabled) return;
            c.QueueImmediate(actions);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            var tooltips = actions
                .SelectMany(a => a.GetTooltips(s))
                .ToList();

            // // TODO: center of hand / noncenter tooltip
            //tooltips.InsertRange(0, new()
            //{
            //
            //});

            return tooltips;
        }

        public override List<Icon> GetIcons(State s)
        {
            var icons = actions
                .Select(a => a.GetIcon(s))
                .Where(i => i != null)
                .Select(i => (Icon)i!)
                .Select(i => new Icon(i.path, i.number, disabled ? Colors.textFaint : i.color, flipY: i.flipY)) // kinda annoying that i have to do this
                .ToList();

            icons.Insert(0, new Icon(
                isCenter
                    ? ModEntry.Instance.sprites["icon_card_is_centered"].Sprite
                    : ModEntry.Instance.sprites["icon_card_is_not_centered"].Sprite,
                null,
                Colors.textMain
            ));

            return icons;
        }
    }
}
