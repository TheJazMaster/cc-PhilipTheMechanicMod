using daisyowl.text;
using FSPRO;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using PhilipTheMechanic.artifacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    [HarmonyPatch(typeof(Card))]
    public class RedrawStatusController
    {
        private static void HandleRedraw(G g, Card card, bool free = false)
        {
            if (g.state.route is Combat c)
            {
                // find toolbox
                var ownedEndlessToolbox = g.state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(EndlessToolbox)).FirstOrDefault() as EndlessToolbox;
                bool activateToolbox = ownedEndlessToolbox != null && ownedEndlessToolbox.counter > 0;

                // subtract cost
                var redrawAmount = g.state.ship.Get((Status)MainManifest.statuses["redraw"].Id);
                if (!free) g.state.ship.Set((Status)MainManifest.statuses["redraw"].Id, redrawAmount - 1);

                // actually do the redraw
                DiscardFromHand(g.state, card);
                c.DrawCards(g.state, activateToolbox ? 2 : 1);

                // handle toolbox visuals
                if (activateToolbox)
                {
                    ownedEndlessToolbox.counter--;
                    ownedEndlessToolbox.Pulse();
                }

                // tell the shout system what just happened
                (g.state.route as Combat).QueueImmediate(new ADummyAction()
                {
                    dialogueSelector = "JustDidRedraw"
                });

                // update the other cards in hand
                foreach (Card otherCard in c.hand)
                {
                    if (otherCard is ModifierCard mc) { mc.OnOtherCardDiscardedWhileThisWasInHand(g.state, c); }
                }
            }
        }

        // Modified from Combat.DiscardHand
        public static void DiscardFromHand(State s, Card card, bool silent = false)
        {
            if (s.route is Combat c)
            {
                c.hand.Remove(card);
                card.waitBeforeMoving = 0.05;
                card.OnDiscard(s, c);
                c.SendCardToDiscard(s, card);

                if (!silent)
                {
                    Audio.Play(Event.CardHandling);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.Render))]
        public static void HarmonyPostfix_Card_Render(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            State state = fakeState ?? g.state;

            if (state.route is Combat c && c.routeOverride != null && !c.eyeballPeek) { return; }
            if (state.route is not Combat) { return; } // should never hit this case
            if (__instance.drawAnim != 1) { return; }

            // checking for isUnplayableModCard here allows us to do the Hot Chocolate logic later
            bool hasRedraw = state.ship.Get((Status)MainManifest.statuses["redraw"].Id) > 0;
            bool isUnplayableModCard = __instance is ModifierCard && __instance.GetDataWithOverrides(state).unplayable;
            if (!hasRedraw && !isUnplayableModCard) { return; }

            // logic for Hot Chocolate artifact
            var ownedHotChocolate = g.state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(HotChocolate)).FirstOrDefault() as HotChocolate;
            int unplayableModCardCount = ownedHotChocolate == null ? 0 : (state.route as Combat).hand.Where(c => c is ModifierCard && c.GetDataWithOverrides(state).unplayable).Count();
            bool redrawsForFree = isUnplayableModCard && unplayableModCardCount >= 3;
            if (!hasRedraw && !redrawsForFree) { return; }

            // Draw the button

            int cardIndex = (g.state.route as Combat).hand.IndexOf(__instance);

            double hoverAnim = __instance.hoverAnim;

            Vec vec = posOverride ?? __instance.pos;
            double hoverAnimOffset = hoverAnim * -2.0;
            Rect rect = (__instance.GetScreenRect() + vec + new Vec(0.0, hoverAnimOffset + Mutil.Parabola(__instance.flipAnim) * -10.0 + Mutil.Parabola(Math.Abs(__instance.flopAnim)) * -10.0 * Math.Sign(__instance.flopAnim))).round();
            Rect value = rect;
            if (overrideWidth.HasValue)
            {
                rect.w = overrideWidth.Value;
            }

            Box box = g.Push(null, rect);
            Vec vec2 = box.rect.xy + new Vec(0.0, 1.0);

            // card height is 82, width is 59
            var cardHalfWidth = 59.0 / 2.0;
            var cardHeight = 82.0;
            Rect rect2 = new(cardHalfWidth-19.0/2.0, cardHeight-13.0/2.0 - hoverAnimOffset, 19, 13);
            OnMouseDown omd = new MouseDownHandler(() => HandleRedraw(g, __instance, redrawsForFree));
            // 855026104 is a random int chosen to not overlap with custom button IDs from other mods
            ButtonSprite(g, vec2, rect2, new UIKey((UK)855026104, __instance.uuid, $"redraw_button_for_card_{cardIndex}"), (Spr)MainManifest.sprites["button_redraw"].Id, (Spr)MainManifest.sprites["button_redraw_on"].Id, onMouseDown: omd, gamepadUntargetable: true);

            g.Pop();
        }

        // from https://github.com/Shockah/Cobalt-Core-Mods/blob/master/CrewSelectionHelper/Patches/NewRunOptionsPatches.cs
        public static SharedArt.ButtonResult ButtonSprite(G g, Vec vec2, Rect rect, UIKey key, Spr sprite, Spr spriteHover, Spr? spriteDown = null, Color? boxColor = null, bool inactive = false, bool flipX = false, bool flipY = false, OnMouseDown? onMouseDown = null, bool autoFocus = false, bool noHover = false, bool showAsPressed = false, bool gamepadUntargetable = false, UIKey? leftHint = null, UIKey? rightHint = null)
        {
            bool gamepadUntargetable2 = gamepadUntargetable;
            Box box = g.Push(key, rect, null, autoFocus, inactive, gamepadUntargetable2, ReticleMode.Quad, onMouseDown, null, null, null, 3, rightHint, leftHint);
            Vec xy = box.rect.xy;
            bool flag = !noHover && (box.IsHover() || showAsPressed) && !inactive;
            if (spriteDown.HasValue && box.IsHover() && Input.mouseLeft)
                showAsPressed = true;
            
            g.Pop();

            Draw.Sprite(!showAsPressed ? flag ? spriteHover : sprite : spriteDown ?? spriteHover, xy.x, xy.y, flipX, flipY, 0, null, null, null, null, boxColor);
            SharedArt.ButtonResult buttonResult = default;
            buttonResult.isHover = flag;
            buttonResult.FIXME_isHoverForTooltip = !noHover && box.IsHover();
            buttonResult.v = xy;
            buttonResult.innerOffset = new Vec(0.0, showAsPressed ? 2 : flag ? 1 : 0);
            SharedArt.ButtonResult result = buttonResult;

            return result;
        }
    }
}
