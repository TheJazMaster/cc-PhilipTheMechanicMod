using clay.PhilipTheMechanic.Actions;
using Shockah.Kokoro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic;

internal class KokoroHooksImplementation : IWrappedActionHook
{
    public List<CardAction>? GetWrappedCardActions(CardAction action)
    {
        if (action is ACenterOfHandWrapper centerOfHandWrapper) return centerOfHandWrapper.actions;
        return null;
    }
}
