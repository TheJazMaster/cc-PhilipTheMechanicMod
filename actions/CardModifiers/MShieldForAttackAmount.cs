using clay.PhilipTheMechanic.Controllers;
using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MShieldForAttackAmount : ICardModifier
    {
        public bool tempShield = true;

        public string DialogueTag => "Philip";
        public double Priority => ModifierCardsController.Prioirites.ADD_ACTION;
        public bool RequestsStickyNote() 
        {
            return true;
        }

        public Spr? GetSticker(State s)
        {
            return tempShield 
                ? ModEntry.Instance.sprites["icon_sticker_shield"].Sprite 
                : ModEntry.Instance.sprites["icon_sticker_temp_shield"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return tempShield
                ? new Icon(ModEntry.Instance.sprites["icon_temp_shield_x"].Sprite, null, Colors.textMain)
                : new Icon(ModEntry.Instance.sprites["icon_shield_x"].Sprite, null, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
        {
            int amount = 0;
            foreach (CardAction action in actions)
            {
                if (ModEntry.Instance.KokoroApi != null)
                {
                    var actionsLevel2 = ModEntry.Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(action, true);
                    foreach (var action2 in actionsLevel2) if (action2 is AAttack a) amount += a.damage;
                }
                else
                {
                    if (action is AAttack a) amount += a.damage;
                }
            }

            actions.Add(new AStatus() 
            {
                status = tempShield ? Status.tempShield : Status.shield,
                targetPlayer = true,
                statusAmount = amount,
            });

            return actions;
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
