using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic;

public partial interface IKokoroApi
{
	IActionApi Actions { get; }

	public interface IActionApi
	{
        CardAction MakeHidden(CardAction action, bool showTooltips = false);
        List<CardAction> GetWrappedCardActionsRecursively(CardAction action, bool includingWrapperActions);
		void RegisterWrappedActionHook(IWrappedActionHook hook, double priority);
    }
}

public interface IWrappedActionHook
{
	List<CardAction>? GetWrappedCardActions(CardAction action);
}