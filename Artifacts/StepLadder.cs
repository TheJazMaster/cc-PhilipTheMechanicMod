using System.Collections.Generic;
using clay.PhilipTheMechanic.Cards;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class StepLadder : Artifact, IRegisterableArtifact
{
    public static ArtifactPool[] GetPools() => [ArtifactPool.Boss];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_step_ladder"];

	public override void OnReceiveArtifact(State state)
	{
		(state.GetDialogue()?.actionQueue ?? state.GetCurrentQueue()).Queue(new AAddCard {
			card = new ExtenderMod(),
			destination = CardDestination.Deck
		});
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		new TTCard {
            card = new ExtenderMod()
        },
    ];
}

