using System.Collections.Generic;
using clay.PhilipTheMechanic.Controllers;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class TopMarks : Artifact, IRegisterableArtifact
{
    public bool active = true;
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_top_marks"];

	public override void OnTurnStart(State state, Combat combat)
	{
		active = true;
	}

    internal static void CheckTrigger(State s, Combat c, Card card, int handPosition) {
        if (handPosition < 0 || handPosition >= ModifierCardsController.LastCachedModifiers.Count) return;

        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is TopMarks artifact && artifact.active)
            if (ModifierCardsController.LastCachedModifiers[handPosition].Count > 0) {
                c.Queue(new AStatus {
                    status = Status.drawNextTurn,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = artifact.Key()
                });
                artifact.active = false;
            }
        }
    }

	public override List<Tooltip>? GetExtraTooltips() => [
		.. StatusMeta.GetTooltips(Status.drawNextTurn, 1),
    ];
}

