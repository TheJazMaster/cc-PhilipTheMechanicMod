using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Cards;
using clay.PhilipTheMechanic.Controllers;
using Microsoft.Xna.Framework;
using Shockah.Kokoro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic;

internal class KokoroHooksImplementation : IWrappedActionHook, ICardRenderHook
{
    public List<CardAction>? GetWrappedCardActions(CardAction action)
    {
        if (action is ACenterOfHandWrapper centerOfHandWrapper) return centerOfHandWrapper.actions;
        return null;
    }

    public bool ShouldDisableCardRenderingTransformations(G g, Card card) 
    {
        var s = g.state;
        if (s.route is not Combat c) { return false; }
        if (c.routeOverride != null && !c.eyeballPeek) { return false; }
        if (card.drawAnim != 1) { return false; }

        return ModifierCardsController.ShouldStickyNote
        (
            card, 
            s, 
            card.GetActionsOverridden(s, c), 
            ModifierCardsController.GetCardModifiers(card, s, c)
        );
    }

    public Matrix ModifyNonTextCardRenderMatrix(G g, Card card, List<CardAction> actions)
    {
        if (card is not Nanobots) return Matrix.Identity;
        return Matrix.CreateScale(1.5f);
    }
}
