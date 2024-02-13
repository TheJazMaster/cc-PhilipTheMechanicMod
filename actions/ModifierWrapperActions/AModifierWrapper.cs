using clay.PhilipTheMechanic.Actions.CardModifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions
{
    public class AModifierWrapper : ATooltipDummy
    {
        public required List<ICardModifier> modifiers;
        public bool isFlimsy;
        public int priority;

        public override List<Icon> GetIcons(State s) 
        { 
            var icons = modifiers
                .Select(modifier => modifier.GetIcon(s))
                .Where(icon => icon != null)
                .Select(icon => icon!.Value)
                .ToList() 
                ?? new();

            var ownIcon = GetIcon(s);
            if (ownIcon.HasValue) icons.Insert(0, ownIcon.Value);

            return icons;
        }

        public override List<Tooltip> GetTooltips(State s) 
        { 
            var tooltips = modifiers
                .Select(modifier => modifier.GetTooltips(s))
                .SelectMany(m => m) // flatten
                .ToList()
                ?? new();

            var ownTooltip = GetTooltip(s);
            if (ownTooltip != null) tooltips.Insert(0, ownTooltip);

            return tooltips;
        }

        public virtual bool IsTargeting(Card potetialTarget, Card myOwnerCard, Combat c)
        {
            return false;
        }

        public virtual Tooltip? GetTooltip(State s) { return null; }
    }
}
