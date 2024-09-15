using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic;

public partial interface IKokoroApi
{
	void RegisterCardRenderHook(ICardRenderHook hook, double priority);
}

public interface ICardRenderHook
{
	bool ShouldDisableCardRenderingTransformations(G g, Card card) => false;
    Matrix ModifyNonTextCardRenderMatrix(G g, Card card, List<CardAction> actions) => Matrix.Identity;
}