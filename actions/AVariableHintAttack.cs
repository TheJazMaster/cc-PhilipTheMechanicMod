using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nickel;

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
                new GlossaryTooltip($"action.{GetType().Namespace!}::{GetType().Name}") {
                    TitleColor = Colors.action,
                    Icon = StableSpr.icons_attack,
                    Title = ModEntry.Instance.Localizations.Localize(["action", "AVariableHintAttack", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["action", "AVariableHintAttack", "description"]),
                }
            ];
        }
    }
}
