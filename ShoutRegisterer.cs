using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic
{
    public class ShoutRegisterer
    {
        public static void RegisterShout((string, StoryNode) when, Deck who, string what, string looptag = "neutral")
        {
            if (!StoryNodeIsVanillaMulti(when))
            {
                var node = Mutil.DeepCopy(when.Item2);
                node.allPresent = new() { who.Key() };
                node.lines = new()
                {
                    new RandallMod.CustomSay()
                    {
                        who = who.Key(),
                        Text = what,
                        loopTag = looptag
                    }
                };

                DB.story.all[$"{when.Item1}_{who.Key()}"] = node;
            } 
            else
            {
                // TODO: can I scan the db for items starting with $"{when.Item1}_Multi"?
                // then scan those entries for sayswitches? that'd save a lot of manual hardcoding
                // might be difficult. I'd need to scan for a sayswitch that has at least 1 vanilla crew in it
                // otherwise (in the case of CrabFacts) it'll try to add the reaction to the list of facts
                (DB.story.all["CrabFacts1_Multi_0"].lines[1] as SaySwitch)!.lines.Add(
                    new RandallMod.CustomSay()
                    {
                        who = who.Key(),
                        Text = what,
                        loopTag = looptag
                    }
                );
            }
        }

        private static HashSet<string> VanillaMultis = new()
        {
            "BanditThreats",
            "CrabFacts1",
            "CrabFacts2",
            "CrabFactsAreOverNow",
        };

        public static bool StoryNodeIsVanillaMulti((string, StoryNode) node)
        {
            return VanillaMultis.Contains(node.Item1);
        }
    }
}
