using clay.PhilipTheMechanic.Controllers;
using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MDeleteActions : ICardModifier
    {
        public string DialogueTag => "Philip";
        public double Priority => ModifierCardsController.Prioirites.REMOVE_ALL_ACTIONS;
        public bool MandatesStickyNote() { return true; }
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_no_action"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(ModEntry.Instance.sprites["icon_no_action"].Sprite, null, Colors.textMain);
        }
        public List<CardAction> TransformActions(List<CardAction> actions, State s, Combat c, Card card, bool isRendering)
        {
            return new();
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
