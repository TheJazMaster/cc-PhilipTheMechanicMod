using System.Collections.Generic;
using System.Linq;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions
{
    public abstract class AModifierWrapper : AMultiIconAction
    {
        public required List<CardModifier> modifiers;
        public bool isFlimsy;
        public int priority;

		public override List<CardAction> GetActionsForRendering(State s) 
        { 
            List<CardAction> actions = [];
            var ownIcon = GetIcon(s);
            if (ownIcon.HasValue) {
                actions.Add(new AIconDummy {
                    icon = ownIcon.Value
                });
            }
            actions.AddRange(modifiers.Select(modifier => modifier.GetActionForRendering(s)));
            return actions;
        }

        public override List<Tooltip> GetTooltips(State s) 
        { 
            var tooltips = modifiers
                .Select(modifier => modifier.GetTooltips(s))
                .SelectMany(m => m) // flatten
                .ToList()
                ?? [];

            var ownTooltip = GetTooltip(s);
            if (ownTooltip != null) tooltips.Insert(0, ownTooltip);

            return tooltips;
        }

        public virtual bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1)
        {
            return false;
        }

        public virtual Tooltip? GetTooltip(State s) { return null; }
    }
}
