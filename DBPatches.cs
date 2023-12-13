using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    [HarmonyPatch(typeof(DB))]
    public static class DBPatches
    {
        public delegate void StoryNodeModification(StoryNode node);
        private static List<(string, StoryNodeModification)> modifications = new();
        private static Dictionary<string, List<(string, string)>> addedLocalizations = new();
        
        public static void RegisterStoryNodeModification(string nodeName, StoryNodeModification modification)
        {
            modifications.Add((nodeName, modification));
        }

        public static void RegisterLocalization(string locale, string key, string value)
        {
            if (!addedLocalizations.ContainsKey(locale)) addedLocalizations.Add(locale, new());
            addedLocalizations[locale].Add((key, value));
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(DB.MakeInitQueue))]
        public static void HarmonyPostfix_DB_MakeInitQueue(ref Queue<(string, Action)> __result,  bool preloadSprites = true)
        {
            __result.Enqueue(("modloader editing loaded story.json", new Action(() =>
            {
                foreach(var (nodeName, modification) in modifications)
                {
                    modification.Invoke(DB.story.all[nodeName]);
                }
            })));
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(DB.LoadStringsForLocale))]
        public static void HarmonyPostfix_DB_LoadStringsForLocale(ref Dictionary<string, string>? __result, string locale)
        {
            if (__result == null) return;
            if (!addedLocalizations.ContainsKey(locale)) return;

            foreach (var (key, value) in addedLocalizations[locale])
            {
                __result.Add(key, value);
            }
        }
    }
}
