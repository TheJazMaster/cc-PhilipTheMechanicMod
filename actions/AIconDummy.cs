using HarmonyLib;
using Shockah;
using System;
using System.Collections.Generic;

namespace clay.PhilipTheMechanic.Actions;

public class AIconDummy : ADummyAction
{
    public Icon? icon;
    public override Icon? GetIcon(State s) => icon;
}
