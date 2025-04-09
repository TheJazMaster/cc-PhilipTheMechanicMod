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
    public int lastHandCount = -1;
    public int triggers = 0;
    static readonly int MAX_TRIGGERS = 3;
    public static ArtifactPool[] GetPools() => [ArtifactPool.Common];
    public static Spr GetSpriteForRegistering() => ModEntry.Instance.sprites["artifact_safety_cert"];

	public override int? GetDisplayNumber(State s) => triggers;

	public override Spr GetSprite() =>
		triggers >= MAX_TRIGGERS ? ModEntry.Instance.sprites["artifact_safety_cert_off"] : ModEntry.Instance.sprites["artifact_safety_cert"];

	public override void OnQueueEmptyDuringPlayerTurn(State state, Combat combat)
	{
		if (triggers < MAX_TRIGGERS && combat.hand.Count > 0 && combat.hand.Count != lastHandCount && combat.hand.Last() is ModifierCard) {
            combat.Queue(new ADrawCard {
                count = 1,
                artifactPulse = Key()
            });
            triggers++;
        }
        lastHandCount = combat.hand.Count;
	}

	public override void OnTurnStart(State state, Combat combat)
	{
		triggers = 0;
	}
}