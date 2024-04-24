using clay.PhilipTheMechanic.Controllers;
using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MRetain : ICardModifier
    {
        public string DialogueTag => "Philip";
        public double Priority => ModifierCardsController.Prioirites.MODIFY_DATA_FAVORABLE;
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_retain"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_retain"), null, Colors.textMain);
        }
        public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
        {
            data.retain = true;
            return data;
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
