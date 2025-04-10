using System.Collections.Generic;
using System.Linq;
using Nickel;

namespace clay.PhilipTheMechanic.Actions.ModifierWrapperActions
{
    public abstract class ModCardSelector
    {
        public virtual bool IsTargeting(Card ownerCard, int originIndex, int affectingIndex, Combat c, int range = 1) => false;

        public virtual Icon? GetIcon(State s, bool isFlimsy, bool overwrites) => null;

        public virtual List<Tooltip> GetTooltips(State s, bool isFlimsy, bool overwrites) => [];

        public List<Tooltip> AddAdditionalTooltips(List<Tooltip> existingTooltips, Spr sprite, bool isFlimsy, bool overwrites, string padding = "") {
            if (isFlimsy)
                existingTooltips.Add(new GlossaryTooltip($"modifier.{GetType().Namespace!}::Flimsy") {// + "FlimsyOverwrite"
                    TitleColor = Colors.downside,
                    Icon = sprite,
                    Title = padding + ModEntry.Instance.Localizations.Localize(["action", "Flimsy", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["action", "Flimsy", "description"]),
                });
            if (overwrites)
                existingTooltips.Add(new GlossaryTooltip($"modifier.{GetType().Namespace!}::Overwrite") {// + "FlimsyOverwrite"
                    TitleColor = Colors.downside,
                    Icon = sprite,
                    Title = padding + ModEntry.Instance.Localizations.Localize(["action", "Overwrite", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["action", "Overwrite", "description"]),
                }); 
            return existingTooltips;
        }
    }
}
