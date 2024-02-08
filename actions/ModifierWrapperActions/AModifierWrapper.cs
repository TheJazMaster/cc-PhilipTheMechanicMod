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
        public required List<CardModifier> modifiers;
        public bool isFlimsy;

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
            return modifiers
                .Select(modifier => modifier.GetTooltips(s))
                .SelectMany(m => m) // flatten
                .ToList()
                ?? new();
        }

        public virtual bool IsTargeting(Card potetialTarget, Card myOwnerCard, Combat c)
        {
            return false;
        }
    }
}
