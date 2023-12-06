using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    public class DialogueManifest : IStoryManifest
    {
        public IEnumerable<DependencyEntry> Dependencies => throw new NotImplementedException();

        public DirectoryInfo? GameRootFolder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILogger? Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DirectoryInfo? ModRootFolder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Name => "clay.PhilipTheEngineer.Story";
        public static string Philip = "clay.PhilipTheMechanic.Philip";

        public void LoadManifest(IStoryRegistry storyRegistry)
        {
            LoadShouts(storyRegistry);
        }

        private void LoadShouts(IStoryRegistry storyRegistry)
        {
            RegisterSimpleShout(storyRegistry, "New loop, new ship to take apart!", "NewLoopShout", loopTag: "maniacal", zones: new HashSet<string>() { "zone_first" });
            RegisterSimpleShout(storyRegistry, "Man the engines are running horribly, forget the fight, I'm gonna tune them.", "Engines", loopTag: "unhappy");
            //RegisterSimpleShout(storyRegistry, "Hey, who installed this cockpit without combat-rated safety glass? They should have their license revoked.", "SafetyGlass", loopTag: "unhappy", storyNode: new StoryNode()
            //{
            //    zones = new HashSet<string>() { "zone_first" },
            //    hasWeakPart = true
            //});
            RegisterSimpleShout(storyRegistry, "Tsk, you really should've armored that weak point.", "EnemyWeakPoint", storyNode: new StoryNode()
            {
                enemyHasWeakPart = true
            });
            RegisterSimpleShout(storyRegistry, "Woah your ship, uh... That brittle part needs to be replaced.", "EnemyBrittlePart", storyNode: new StoryNode()
            {
                enemyHasBrittlePart = true
            });
            RegisterSimpleShout(storyRegistry, "Yeeeeeaaah we don't have enough spare parts to fix that.", "DamageTaken", storyNode: new StoryNode()
            {
                minDamageDealtToPlayerThisTurn = 5
            });
            RegisterSimpleShout(storyRegistry, "I hear whirring. Why do I hear whirring?", "HullLow", loopTag: "gameover", storyNode: new StoryNode()
            {
                maxHull = 3,
                maxHullPercent = 0.5
            });
            RegisterSimpleShout(storyRegistry, "Don't worry! I've got a tiny fire extinguisher!", "Heat", storyNode: new StoryNode()
            {
                goingToOverheat = true
            });
            RegisterSimpleShout(storyRegistry, "I knew that fire extinguisher would help.", "DidntHeat", loopTag: "classy", storyNode: new StoryNode()
            {
                wasGoingToOverheatButStopped = true
            });
            RegisterSimpleShout(storyRegistry, "Woah ho!!", "LotsOfDamage", storyNode: new StoryNode()
            {
                minDamageDealtToEnemyThisTurn = 5
            });

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.SelfPropellingCannons",
                node: new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    allPresent = new() { Philip, "goat" },
                    hasArtifacts = new() { "clay.PhilipTheMechanic.Artifacts.SelfPropellingCannons" }
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "goat",
                        What = "Hey Philip, you DID remember to secure the cannon struts, right?",
                        LoopTag = "squint",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Secure the...",
                        Delay = 1,
                        LoopTag = "squint"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Oh. Uh oh.",
                        Delay = 2,
                        LoopTag = "gameover"
                    },
                }
            ));

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.ObjectionableConsoleRemoval",
                node: new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    allPresent = new() { Philip }
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Hey, you guys don't need monitors on your command consoles, right?",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "peri",
                        What = "Yes.",
                        LoopTag = "squint",
                        Delay = 1
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "shard",
                        What = "I don't think so!",
                        Delay = 1
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "hacker",
                        What = "Don't touch my console.",
                        LoopTag = "squint",
                        Delay = 1
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "goat",
                        What = "We do!",
                        LoopTag = "squint",
                        Delay = 1
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "dizzy",
                        What = "Nah!",
                        Delay = 1
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "riggs",
                        What = "Ah... yeah I do.",
                        LoopTag = "squint",
                        Delay = 1
                    },
                }
            ));
        }

        private void RegisterSimpleShout(IStoryRegistry storyRegistry, string shout, string name, string? loopTag = null, StoryNode storyNode = null, HashSet<string>? zones = null)
        {
            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.{name}",
                node: storyNode ?? new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    zones = zones,
                    allPresent = new() { Philip }
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "clay.PhilipTheMechanic.Philip",
                        What = shout,
                        LoopTag = loopTag
                    },
                }
            ));
        }
    }
}
