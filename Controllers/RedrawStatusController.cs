using daisyowl.text;
using FSPRO;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Controllers
{
    [HarmonyPatch(typeof(Card))]
    public class RedrawStatusController
    {
        private static void HandleRedraw(G g, Card card, int index)
        {
            if (g.state.route is not Combat c) return;
            if (c == DB.fakeCombat) return;

            var apiImplementation = ((ApiImplementation)ModEntry.Instance.Api);

            // TODO: add an API hook here for implementing artifacts, and implement scrap magnet and endless tool 
            //// find toolbox
            //var ownedEndlessToolbox = g.state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(EndlessToolbox)).FirstOrDefault() as EndlessToolbox;
            //bool activateToolbox = ownedEndlessToolbox != null && ownedEndlessToolbox.counter > 0;
            //if (activateToolbox)
            //{
            //    drawAmount = 2;
            //    ownedEndlessToolbox.counter--;
            //    ownedEndlessToolbox.Pulse();
            //}

            //// find scrap magnet
            //if (index == 0 && !free)
            //{
            //    var ownedScrapMagnet = g.state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(ScrapMagnet)).FirstOrDefault() as ScrapMagnet;
            //    bool activateScrapMagnet = ownedScrapMagnet != null && ownedScrapMagnet.counter > 0;
            //    if (activateScrapMagnet)
            //    {
            //        free = true;
            //        ownedScrapMagnet.counter--;
            //        ownedScrapMagnet.Pulse();
            //    }
            //}

            // subtract cost
            int cost = 1;
            foreach ((IRedrawCostHook, double) hook in apiImplementation.RedrawCostHooks) cost = hook.Item1.RedrawCost(cost, card, g.state, c);

            var redrawAmount = g.state.ship.Get(ModEntry.Instance.RedrawStatus.Status);
            g.state.ship.Set(ModEntry.Instance.RedrawStatus.Status, redrawAmount - 1);

            // actually do the redraw
            DiscardFromHand(g.state, card);
            c.DrawCards(g.state, 1);

            // tell the shout system what just happened
            c.QueueImmediate(new ADummyAction()
            {
                dialogueSelector = ".JustDidRedraw"
            });

            // notify all registered hooks that redraw just happened
            foreach ((IOnRedrawHook, double) hook in apiImplementation.OnRedrawHooks) hook.Item1.OnRedraw(card, g.state, c);
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

        public static bool ShouldDrawRedrawArrow(Card card, State state)
        {
            if (state.route is not Combat combat) return false;
            if (combat == DB.fakeCombat) return false;

            var apiImplementation = ((ApiImplementation)ModEntry.Instance.Api);
            var anyHook = apiImplementation
                .AllowRedrawHooks
                .Select(hook => hook.AllowRedraw(card, state, combat))
                .Any();

            if (anyHook) return true;

            bool hasRedraw = state.ship.Get(ModEntry.Instance.RedrawStatus.Status) > 0;
            if (hasRedraw) return true;
            return false;

            // TODO: implement hot chocolate and scrap magnet using the api hooks
            //bool isUnplayableModCard = __instance is ModifierCard && __instance.GetDataWithOverrides(state).unplayable;

            //bool HOT_CHOCOLATE_CONDITION = isUnplayableModCard;
            //bool SCRAP_MAGNET_CONDITION = cardIndex == 0;

            //if (!HOT_CHOCOLATE_CONDITION && !SCRAP_MAGNET_CONDITION) { return false; }

            //// logic for Hot Chocolate artifact
            //var ownedHotChocolate = g.state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(HotChocolate)).FirstOrDefault() as HotChocolate;
            //int unplayableModCardCount = ownedHotChocolate == null ? 0 : (state.route as Combat).hand.Where(c => c is ModifierCard && c.GetDataWithOverrides(state).unplayable).Count();
            //bool redrawsForFree = ownedHotChocolate != null && HOT_CHOCOLATE_CONDITION && unplayableModCardCount >= 3;

            //// logic for Scrap Magnet artifact
            //var ownedScrapMagnet = g.state.EnumerateAllArtifacts().Where((Artifact a) => a.GetType() == typeof(ScrapMagnet)).FirstOrDefault() as ScrapMagnet;
            //bool activateScrapMagnet = ownedScrapMagnet != null && ownedScrapMagnet.counter > 0 && SCRAP_MAGNET_CONDITION;
            ////redrawsForFree = redrawsForFree || activateScrapMagnet;

            //if (!redrawsForFree && !activateScrapMagnet) { return false; }
            //return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Card.Render))]
        public static void RenderRedrawButtons(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false, bool isInCombatHand = false)
        {
            State state = fakeState ?? g.state;

            if (!ModifierCardsController.ModifiersCurrentlyApply(state, __instance)) return; // check to make sure the game is in the right state to even be drawing these arrows to begin with
            if (!ShouldDrawRedrawArrow(__instance, state)) return; // check to see if there's even a possibility we should draw the redraw arrow
            
            Combat c = (state.route as Combat)!;
            int cardIndex = c.hand.IndexOf(__instance);

            // Draw the button

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
            Rect rect2 = new(cardHalfWidth - 19.0 / 2.0, cardHeight - 13.0 / 2.0 - hoverAnimOffset, 19, 13);
            OnMouseDown omd = new MouseDownHandler(() => HandleRedraw(g, __instance, cardIndex));
            // 855026104 is a random int chosen to not overlap with custom button IDs from other mods
            ButtonSprite(g, vec2, rect2, new UIKey((UK)855026104, __instance.uuid, $"redraw_button_for_card_{cardIndex}"), ModEntry.Instance.sprites["button_redraw"].Sprite, ModEntry.Instance.sprites["button_redraw_on"].Sprite, onMouseDown: omd);

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
