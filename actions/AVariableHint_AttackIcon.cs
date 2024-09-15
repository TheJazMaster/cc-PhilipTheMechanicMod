using Shockah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic.Actions
{
    internal class AVariableHintAttack : AVariableHint
    {
        public AVariableHintAttack() {
            hand = true;
        }
        
        public override Icon? GetIcon(State s)
        {
            return new Icon(StableSpr.icons_attack, null, Colors.textMain);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            return [
                new CustomTTGlossary(
                    CustomTTGlossary.GlossaryType.action,
                    () => StableSpr.icons_attack,
                    () => ModEntry.Instance.Localizations.Localize(["action", "AVariableHintAttack", "name"]),
                    () => ModEntry.Instance.Localizations.Localize(["action", "AVariableHintAttack", "description"]),
                    key: GetType().FullName ?? GetType().Name
                )
            ];
        }
    }
}
