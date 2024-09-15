using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Cards;
using clay.PhilipTheMechanic.Controllers;
using Microsoft.Xna.Framework;
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
        if (s.route is not Combat c) return false;
        if (c.routeOverride != null && !c.eyeballPeek) return false;
        if (card.drawAnim != 1) return false;
        int index = c.hand.IndexOf(card);
        if (index < 0 || index >= c.hand.Count) return false;

        ModifierCardsController.CalculateCardModifiers(s, c);
        return ModifierCardsRenderingController.ShouldStickyNote(card, s, c, ModifierCardsController.LastCachedModifiers[index], index);
    }

    public Matrix ModifyNonTextCardRenderMatrix(G g, Card card, List<CardAction> actions)
    {
        if (card is not Nanobots) return Matrix.Identity;
        return Matrix.CreateScale(1.5f);
    }
}
