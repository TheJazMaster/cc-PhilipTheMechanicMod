﻿using clay.PhilipTheMechanic.Actions.ModifierWrapperActions;
using clay.PhilipTheMechanic.Controllers;
using Microsoft.Extensions.Logging;
using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MAttacksPierce : ICardModifier
    {
        public string DialogueTag => "Philip";
        public double Priority => ModifierCardsController.Prioirites.MODIFY_ALL_ACTIONS;

        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_piercing"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_attackPiercing"), null, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
        {
            foreach (var action in actions)
            {
                if (ModEntry.Instance.KokoroApi != null)
                {
                    var actionsLevel2 = ModEntry.Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(action, true);
                    foreach (var action2 in actionsLevel2) ModifyAction(action2);
                }
                else
                {
                    ModifyAction(action);
                }
            }
            return actions;
        }

        private void ModifyAction(CardAction action)
        {
            if (action is AAttack aattack)
            {
                aattack.piercing = true;
            }
        }

        public List<Tooltip> GetTooltips(State s)
        {
            return [
                new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.actionMisc,
                    () => GetIcon(s)!.Value!.path,
                    () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "name"]),
                    () => ModEntry.Instance.Localizations.Localize(["modifier", GetType().Name, "description"]),
                    key: GetType().FullName ?? GetType().Name
                )
            ];
        }
    }
}
