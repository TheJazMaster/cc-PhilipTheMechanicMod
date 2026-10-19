using System.Collections.Generic;
using System.Linq;
using clay.PhilipTheMechanic.Actions.CardModifiers;
using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;

namespace clay.PhilipTheMechanic.Actions
{
    public class AModifierWrapper : AMultiIconAction
    {
        public required ModCardSelector selector;
        public required List<CardModifier> modifiers { set; private get; }
        public bool isFlimsy;
        public bool overwrites;

		public sealed override Icon? GetIcon(State s) => selector.GetIcon(s, isFlimsy, overwrites);

        public List<CardModifier> GetCardModifiers() {
            if (overwrites) return [
                new MDeleteActions(),
                ..modifiers,
            ];
            return modifiers;
        }

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

        public override List<Tooltip> GetTooltips(State s) => [
            .. selector.GetTooltips(s, isFlimsy, overwrites),
            .. modifiers
                .Select(modifier => modifier.GetTooltips(s))
                .SelectMany(m => m) // flatten
                .ToList()
                ?? [],
        ];
    }
}
