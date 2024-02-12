using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Shockah.Kokoro;

public partial interface IKokoroApi
{
	void RegisterCardRenderHook(ICardRenderHook hook, double priority);
}

public interface ICardRenderHook
{
	bool ShouldDisableCardRenderingTransformations(G g, Card card) => false;
}