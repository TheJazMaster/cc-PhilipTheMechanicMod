using clay.PhilipTheMechanic.Cards;
using clay.PhilipTheMechanic.Controllers;
using HarmonyLib;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace clay.PhilipTheMechanic.Artifacts;

internal sealed class SafetyCertification : Artifact, IRegisterableArtifact
{
    int lastHandCount = -1;
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_safety_cert"];

	public override void OnQueueEmptyDuringPlayerTurn(State state, Combat combat)
	{
		if (combat.hand.Count > 0 && combat.hand.Count != lastHandCount && combat.hand.Last() is ModifierCard) {
            combat.Queue(new ADrawCard {
                count = 1,
                artifactPulse = Key()
            });
        }
        lastHandCount = combat.hand.Count;
	}
}