using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions.CardModifiers;
    
public abstract class BasicCardModifier : CardModifier
{
	public override bool IgnoresFlimsy => false;

    public abstract Icon? GetIcon();

    public override CardAction GetActionForRendering(State s) => new AIconDummy {
        icon = GetIcon()
    };
}