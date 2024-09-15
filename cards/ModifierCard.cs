using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using clay.PhilipTheMechanic.Controllers;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Cards;


internal abstract class ModifierCard : Card
{
    private void FlipIfNeeded(List<AModifierWrapper> list) {
        if (!flipped) return;

        foreach (AModifierWrapper wrapper in list) {
            if (wrapper is ADirectionalCardModifierWrapper dir)
                dir.left = !dir.left;
        }
    }

	public override void OnFlip(G g)
	{
		base.OnFlip(g);
        ModifierCardsController.InvalidateCache();
	}

	public List<AModifierWrapper> GetModifierActionsOverriden(State s, Combat c) {
        List<AModifierWrapper> mods = GetModifierActions(s, c);
        FlipIfNeeded(mods);
        return mods;
    }
	public abstract List<AModifierWrapper> GetModifierActions(State s, Combat c);
	public virtual List<CardAction> GetOtherActions(State s, Combat c) => [];
    public sealed override List<CardAction> GetActions(State s, Combat c) => 
        [.. GetModifierActionsOverriden(s, c), .. GetOtherActions(s, c)];
}