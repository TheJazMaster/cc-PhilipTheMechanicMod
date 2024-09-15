using System.Linq;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class ScrapMagnet : Artifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Boss];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_electromagnet"];
    public static readonly int DEFAULT_COUNTER = 2;
    public int counter = DEFAULT_COUNTER;

    public override int? GetDisplayNumber(State s)
    {
        return counter;
    }

	public override void OnTurnStart(State state, Combat combat)
	{
		counter = DEFAULT_COUNTER;
	}
}

public class ScrapMagnetHook : IRedrawStatusHook
{
    public bool? CanRedraw(State state, Combat combat, Card card)
    {
		if (state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(ScrapMagnet)).FirstOrDefault() is not ScrapMagnet ownedScrapMagnet) return null;

		if (ownedScrapMagnet.counter > 0 && combat.hand.IndexOf(card) == 0) return true;

        return null;
    }

    public bool PayForRedraw(State state, Combat combat, Card card, IRedrawStatusHook possibilityHook) {
        if (possibilityHook is not ScrapMagnetHook) return false;
        if (state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(ScrapMagnet)).FirstOrDefault() is not ScrapMagnet ownedScrapMagnet) return false;
        ownedScrapMagnet.counter--;
        return true;
    }
}