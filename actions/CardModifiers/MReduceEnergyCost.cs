using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions.CardModifiers
{
    public class MReduceEnergyCost : ICardModifier
    {
        public Spr? GetSticker(State s)
        {
            return ModEntry.Instance.sprites["icon_sticker_energy_discount"].Sprite;
        }
        public Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_discount"), null, Colors.textMain);
        }
        public CardData TransformData(CardData data, State s, Combat c, Card card, bool isRendering)
        {
            data.cost = Math.Max(0, data.cost - 1);
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
