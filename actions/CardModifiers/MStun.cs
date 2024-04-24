using clay.PhilipTheMechanic.Controllers;
using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MStun : ICardModifier
    {
        public string DialogueTag => "Philip";
        public double Priority => ModifierCardsController.Prioirites.MODIFY_ALL_ACTIONS;

        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_stun"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_stun"), null, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
        {
            foreach (var action in actions)
            {
                if (action is AAttack aattack)
                {
                    aattack.stunEnemy = true;
                }
            }
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
