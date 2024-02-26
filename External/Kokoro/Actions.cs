using System;
using System.Collections.Generic;

namespace Shockah.Kokoro;

public partial interface IKokoroApi
{
	IActionApi Actions { get; }

	public interface IActionApi
	{
		List<CardAction> GetWrappedCardActionsRecursively(CardAction action, bool includingWrapperActions);
		void RegisterWrappedActionHook(IWrappedActionHook hook, double priority);
    }
}

public interface IWrappedActionHook
{
	List<CardAction>? GetWrappedCardActions(CardAction action);
}