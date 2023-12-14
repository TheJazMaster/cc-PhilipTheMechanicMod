using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilipTheMechanic
{
    public class DialogueManifest : IStoryManifest
    {
        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[0];

        public DirectoryInfo? GameRootFolder { get; set; }
        public Microsoft.Extensions.Logging.ILogger? Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }

        public string Name => "clay.PhilipTheEngineer.Story";
        public static string Philip = "clay.PhilipTheMechanic.PhilipDeck"; // "clay.PhilipTheMechanic.Philip";

        public void LoadManifest(IStoryRegistry storyRegistry)
        {
            LoadShouts(storyRegistry);
            LoadOutsideOfCombatConversations(storyRegistry);
            LoadShopConversations(storyRegistry);
        }

        private void LoadOutsideOfCombatConversations(IStoryRegistry storyRegistry)
        {
            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.IsaacHardhat",
                node: new StoryNode()
                {
                    type = NodeType.@event,
                    oncePerRun = true,
                    lookup = new HashSet<string>() { "after_any" },
                    allPresent = new() { Philip, "goat" },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "goat",
                        What = "Hey Philip, how come I never see you wearing a hardhat? Shouldn't that be required, for safety?"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "Nothing falls when you've got the gravity off!",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "goat",
                        What = "Ah... that makes sense.",
                        LoopTag = "shy"
                    },
                }
            ));

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.IsaacCannonStruts",
                node: new StoryNode()
                {
                    type = NodeType.@event,
                    oncePerRun = true,
                    lookup = new HashSet<string>() { "after_any" },
                    allPresent = new() { Philip, "goat" },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "goat",
                        What = "Hey, are you sure that engine will hold?"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "Certain!",
                        LoopTag = "classy"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "goat",
                        What = "Because without a strut there, and some plating there... it'll rip itself out of the ship.",
                        LoopTag = "squint"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "... oh... I see.",
                        LoopTag = "whatisthat"
                    },
                }
            ));

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.PeriEfficiency",
                node: new StoryNode()
                {
                    type = NodeType.@event,
                    oncePerRun = true,
                    lookup = new HashSet<string>() { "after_any" },
                    allPresent = new() { Philip, "peri" },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "peri",
                        What = "Philip, this cannon will work at higher efficiency, right?"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "Yep, I'm 80% confident it won't explode this time."
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "Wait I misunderstood your question.",
                        LoopTag = "squint"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "peri",
                        What = "...",
                        LoopTag = "squint"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "My answer's still the same though.",
                        LoopTag = "classy"
                    },
                }
            ));

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.DizzyExplosion",
                node: new StoryNode()
                {
                    type = NodeType.@event,
                    oncePerRun = true,
                    lookup = new HashSet<string>() { "after_any" },
                    allPresent = new() { Philip, "dizzy" },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Hey Dizzy! Want to see a phosphorus explosion?"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = "dizzy",
                        What = "Do I ever!"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "... from behind the observation window.",
                        LoopTag = "squint"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "I'm in here because I'm certified to have my fur burnt off.",
                        LoopTag = "maniacal"
                    },
                }
            ));

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.BooksRetain",
                node: new StoryNode()
                {
                    type = NodeType.@event,
                    oncePerRun = true,
                    lookup = new HashSet<string>() { "after_any" },
                    allPresent = new() { Philip, "shard" },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "shard",
                        What = "Hi Mr. Philip! I heard you have a sticker collection!"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "Sure do!",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "shard",
                        What = "Can I have one?"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "Sure! Here take... this one.",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "shard",
                        What = "Thank you Mr. Philip!"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = Philip,
                        What = "She's adorable. I hope that retain sticker works.",
                        LoopTag = "proud"
                    },
                }
            ));
        }

        private void LoadShopConversations(IStoryRegistry storyRegistry)
        {
            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.ShopNanobots",
                node: new StoryNode()
                {
                    type = NodeType.@event,
                    lookup = new HashSet<string>() { "shopBefore" },
                    bg = "BGShop",
                    allPresent = new() { Philip },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = "nerd",
                        What = "Hey Philip, you don't have nanobots on you again, do you?"
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Ahahahaha.... no?",
                        LoopTag = "sheepish"
                    },
                    new Jump()
                    {
                        key = "NewShop"
                    }
                }
            ));

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.ShopIllegalParts",
                node: new StoryNode()
                {
                    type = NodeType.@event,
                    lookup = new HashSet<string>() { "shopBefore" },
                    bg = "BGShop",
                    allPresent = new() { Philip },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Hey Cleo! Got any parts for me?",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = "nerd",
                        What = "Nothing I can sell you."
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Flipped = true,
                        Who = "nerd",
                        What = "Legally."
                    },
                    new Jump()
                    {
                        key = "NewShop"
                    }
                }
            ));
        }

        private void LoadShouts(IStoryRegistry storyRegistry)
        {
            RegisterSimpleShout(storyRegistry, "New loop, new ship to take apart!", "NewLoopShout", loopTag: "maniacal", zones: new HashSet<string>() { "zone_first" }, oncePerRun: true);
            RegisterSimpleShout(storyRegistry, "Man, the engines are running horribly. Forget the fight, I'm gonna tune them.", "Engines", loopTag: "gone", oncePerRun: true);
            RegisterSimpleShout(storyRegistry, "Hey, who installed this cockpit without combat-rated safety glass? They should have their license revoked.", "SafetyGlass", loopTag: "squint", storyNode: StandardShoutHooks.Relevance3.ArtifactHardmode, oncePerRun: true);
            RegisterSimpleShout(storyRegistry, "Tsk, you really should've armored that weak point.", "EnemyWeakPoint", loopTag: "classy", storyNode: StandardShoutHooks.Relevance7.EnemyHasWeakness);
            RegisterSimpleShout(storyRegistry, "Woah your ship, uh... That brittle part needs to be replaced.", "EnemyBrittlePart", storyNode: StandardShoutHooks.Relevance7.EnemyHasBrittle);
            RegisterSimpleShout(storyRegistry, "Yeeeeeaaah we don't have enough spare parts to fix that.", "DamageTaken", loopTag: "squint", storyNode: StandardShoutHooks.Relevance7.ThatsALotOfDamageToUs);
            RegisterSimpleShout(storyRegistry, "I hear whirring. Why do I hear whirring?", "HullLow", loopTag: "gameover", storyNode: StandardShoutHooks.Relevance8.Duo_AboutToDieAndLoop);
            RegisterSimpleShout(storyRegistry, "Don't worry! I've got a tiny fire extinguisher!", "Heat", storyNode: new StoryNode()
            {
                goingToOverheat = true
            });
            RegisterSimpleShout(storyRegistry, "I knew that fire extinguisher would help.", "OverheatAnyoneFix", loopTag: "classy", storyNode: new StoryNode()
            {
                wasGoingToOverheatButStopped = true
            });
            RegisterSimpleShout(storyRegistry, "Woah ho!!", "LotsOfDamage", storyNode: StandardShoutHooks.Relevance6.WeDidOverThreeDamage);
            RegisterSimpleShout(storyRegistry, "Everything's coming together!", "WeDidOverFiveDamage", loopTag: "maniacal", storyNode: StandardShoutHooks.Relevance7.WeDidOverFiveDamage);
            RegisterSimpleShout(storyRegistry, "Thank you shields!", "PlayerCompletelyBlocked", storyNode: StandardShoutHooks.Relevance6.WeGotShotButTookNoDamage);
            RegisterSimpleShout(storyRegistry, "Man, these engines are so good.", "PlayerEscaped", storyNode: StandardShoutHooks.Relevance6.WeDontOverlapWithEnemyAtAll);
            RegisterSimpleShout(storyRegistry, "Aw man, why'd it have to be corrosion?", "ImCorroded", loopTag: "unhappy", storyNode: StandardShoutHooks.Relevance6.WeAreCorroded);
            RegisterSimpleShout(storyRegistry, "Oooh... fixing that corrosion damage is not gonna be fun for you.", "TheyGotCorroded", loopTag: "squint", storyNode: StandardShoutHooks.Relevance5.TheyGotCorroded);
            RegisterSimpleShout(storyRegistry, "I really gotta get better at organization.", "Trash", loopTag: "sheepish", storyNode: StandardShoutHooks.Relevance6.HandOnlyHasTrashCards);
            RegisterSimpleShout(storyRegistry, "Out of tools...", "Energy", loopTag: "unhappy", storyNode: StandardShoutHooks.Relevance8.EmptyHandWithEnergy);
            RegisterSimpleShout(storyRegistry, "Solid hit!", "JustHit", storyNode: StandardShoutHooks.Relevance8.JustHitGeneric);
            RegisterSimpleShout(storyRegistry, "Woah... Max, who's your parts supplier?", "ArtifactTridimensionalCockpit", storyNode: StandardShoutHooks.Relevance7.ArtifactTridimensionalCockpit);
            RegisterSimpleShout(storyRegistry, "And here I thought I'd never get to use that woodworking class!", "ArtifactTiderunner", loopTag: "excited", storyNode: StandardShoutHooks.Relevance7.ArtifactTiderunner);
            RegisterSimpleShout(storyRegistry, "Easy repair!", "WeGotHurtButNotTooBad", loopTag: "classy", storyNode: StandardShoutHooks.Relevance6.WeGotHurtButNotTooBad);
            RegisterSimpleShout(storyRegistry, "This is great, I don't even need to fix that.", "ArtifactNanofiberHull1", storyNode: StandardShoutHooks.Relevance5.ArtifactNanofiberHull1);
            RegisterSimpleShout(storyRegistry, "Dang. Can we get extra nanofibers? No?", "ArtifactNanofiberHull2", storyNode: StandardShoutHooks.Relevance4.ArtifactNanofiberHull2);
            RegisterSimpleShout(storyRegistry, "Woah, I love these engines.", "ArtifactJetThrusters", loopTag: "excited", storyNode: StandardShoutHooks.Relevance5.ArtifactJetThrusters);
            RegisterSimpleShout(storyRegistry, "And that's why you should always install armor on your ship.", "BlockedALotOfAttacksWithArmor", loopTag: "classy", storyNode: StandardShoutHooks.Relevance4.BlockedALotOfAttacksWithArmor);
            RegisterSimpleShout(storyRegistry, "That... cannot be a good noise I just heard.", "WeJustOverheated", loopTag: "squint", storyNode: StandardShoutHooks.Relevance2.WeJustOverheated);


            RegisterModifiedCardShout(storyRegistry, "shard",
                characterText: "The power of friendship!",
                characterLooptag: "paws",
                philipText: "Get 'em Books!!",
                philipLoopTag: "excited");
            RegisterModifiedCardShout(storyRegistry, "riggs",
                characterText: "Woah! That was cool!",
                philipText: "There's more where that came from!");
            RegisterModifiedCardShout(storyRegistry, "hacker",
                characterText: "Hey, how'd you bypass my security?",
                characterLooptag: "squint",
                philipText: "I have my ways.",
                philipLoopTag: "classy");
            RegisterModifiedCardShout(storyRegistry, "dizzy",
                characterText: "How'd you do that?",
                characterLooptag: "surprised",
                philipText: "Hopes, dreams, and a screwdriver.");
            RegisterModifiedCardShout(storyRegistry, "peri",
                characterText: "Good work, Philip.");
            RegisterModifiedCardShout(storyRegistry, "comp",
                characterText: "I don't remember giving you write privlidges...",
                characterLooptag: "squint",
                philipText: "Please don't eject me.",
                philipLoopTag: "sheepish");


            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.FreeRedraw",
                node: new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    oncePerRun = true,
                    lookup = new() { "JustDidRedraw" },
                    allPresent = new() { Philip },
                    hasArtifacts = new() { "clay.PhilipTheMechanic.Artifacts.HotChocolate" }
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Ahhh, hot chocolate makes reorganizing so much easier.",
                        LoopTag = "hotchocolate"
                    },
                }
            ));


            DBPatches.RegisterStoryNodeModification("ShopKeepBattleInsult", (node) =>
            {
                (node.lines[0] as SaySwitch).lines.Add(new Say()
                {
                    who = Philip,
                    hash = "Philip",
                    loopTag = "whatisthat"
                });
                DBPatches.RegisterLocalization("en", "ShopKeepBattleInsult:Philip", "...");
            });

            DBPatches.RegisterStoryNodeModification("CrabFacts1_Multi_0", (node) =>
            {
                // lines[0] are the crab facts, lines[1] are the crew's responses
                (node.lines[1] as SaySwitch).lines.Add(new Say()
                {
                    who = Philip,
                    hash = "Philip"
                });
                DBPatches.RegisterLocalization("en", "CrabFacts1_Multi_0:Philip", "Wow! Now I want to be a crabologist.");
            });
            DBPatches.RegisterStoryNodeModification("CrabFacts2_Multi_0", (node) =>
            {
                // lines[0] are the crab facts, lines[1] are the crew's responses
                (node.lines[1] as SaySwitch).lines.Add(new Say()
                {
                    who = Philip,
                    hash = "Philip",
                    loopTag = "squint"
                });
                DBPatches.RegisterLocalization("en", "CrabFacts2_Multi_0:Philip", "I don't get it.");
            });
            DBPatches.RegisterStoryNodeModification("CrabFactsAreOverNow_Multi_0", (node) =>
            {
                // lines[0] are the crab facts, lines[1] are the crew's responses
                (node.lines[1] as SaySwitch).lines.Add(new Say()
                {
                    who = Philip,
                    hash = "Philip",
                    loopTag = "classy"
                });
                DBPatches.RegisterLocalization("en", "CrabFactsAreOverNow_Multi_0:Philip", "Guess I'll have to find my own now.");
            });
            DBPatches.RegisterStoryNodeModification("Soggins_Missile_Shout_1", (node) =>
            {
                (node.lines[1] as SaySwitch).lines.Add(new Say()
                {
                    who = Philip,
                    hash = "Philip",
                    loopTag = "laugh"
                });
                DBPatches.RegisterLocalization("en", "Soggins_Missile_Shout_1:Philip", "Reminds me of when I was an apprentice!");
            });
            DBPatches.RegisterStoryNodeModification("SogginsEscapeIntent_1", (node) =>
            {
                (node.lines[1] as SaySwitch).lines.Add(new Say()
                {
                    who = Philip,
                    hash = "Philip",
                });
                DBPatches.RegisterLocalization("en", "SogginsEscapeIntent_1:Philip", "Classic solution.");
            });
            DBPatches.RegisterStoryNodeModification("WeJustGainedHeatAndDrakeIsHere_Multi_0", (node) =>
            {
                (node.lines[0] as SaySwitch).lines.Add(new Say()
                {
                    who = Philip,
                    hash = "Philip",
                    loopTag = "unhappy"
                });
                DBPatches.RegisterLocalization("en", "WeJustGainedHeatAndDrakeIsHere_Multi_0:Philip", "You're not making my job easy here, Drake.");
            });


            //
            // RANDOM MID-COMBAT CONVERSATIONS
            //

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.KickTheirButt",
                node: new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    playerShotJustHit = true,
                    minDamageDealtToEnemyThisAction = 1,
                    allPresent = new() { Philip, "shard" },
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "shard",
                        What = "KICK THEIR BUTT!",
                        LoopTag = "squint",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Yeah, you tell 'em Books!!",
                        LoopTag = "excited"
                    },
                }
            ));
            
            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.SelfPropellingCannons",
                node: new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    oncePerRun = true,
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
                    oncePerRun = true,
                    allPresent = new() { Philip }
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = "Hey, you guys don't need monitors on your command consoles, right?",
                    },
                    // TODO: this needs to be in a sayswitch
                    new ExternalStory.ExternalSay()
                    {
                        Who = "peri",
                        What = "Yes.",
                        LoopTag = "squint",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "shard",
                        What = "I don't think so!",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "hacker",
                        What = "Don't touch my console.",
                        LoopTag = "squint",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "goat",
                        What = "Please don't take my monitor.",
                        LoopTag = "squint",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "dizzy",
                        What = "Nah!",
                    },
                    new ExternalStory.ExternalSay()
                    {
                        Who = "riggs",
                        What = "Ah... yeah I do.",
                        LoopTag = "squint",
                    },
                }
            ));
        }

        private void RegisterModifiedCardShout(IStoryRegistry storyRegistry, string character, string? characterText = null, string? characterLooptag = null, string? philipText = null, string? philipLoopTag = null)
        {
            List<object> instructions = new();
            if (characterText != null) 
                instructions.Add(new ExternalStory.ExternalSay()
                {
                    Who = character,
                    What = characterText,
                    LoopTag = characterLooptag,
                });

            if (philipText != null)
                instructions.Add(new ExternalStory.ExternalSay()
                {
                    Who = Philip,
                    What = philipText,
                    LoopTag = philipLoopTag
                });

            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.{character}Card_ModifiedByPhilip",
                node: new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    allPresent = new() { Philip, character },
                    oncePerRun = true,
                    lookup = new() { $"{character}Card_ModifiedByPhilip" }
                },
                instructions: instructions
            ));
        }

        private void RegisterSimpleShout(IStoryRegistry storyRegistry, string shout, string name, string? loopTag = null, StoryNode storyNode = null, HashSet<string>? zones = null, bool oncePerRun = false)
        {
            storyRegistry.RegisterStory(new ExternalStory(
                $"{Name}.{name}",
                node: storyNode ?? new StoryNode()
                {
                    type = NodeType.@combat,
                    priority = false,
                    zones = zones,
                    oncePerRun = oncePerRun,
                    allPresent = new() { Philip }
                },
                instructions: new List<object>()
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = Philip,
                        What = shout,
                        LoopTag = loopTag
                    },
                }
            ));
        }
    }
}
