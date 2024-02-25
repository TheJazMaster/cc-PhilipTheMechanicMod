using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    internal class AVariableHint_AttackIcon : AVariableHint
    {
        public override Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_attack"), null, Colors.textMain);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            return [
                new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => Enum.Parse<Spr>("icons_attack"),
                    () => ModEntry.Instance.Localizations.Localize(["action", "AVariableHint_AttackIcon", "name"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "AVariableHint_AttackIcon", "description"]),
                    key: GetType().FullName ?? GetType().Name
                )
            ];
        }
    }
}
