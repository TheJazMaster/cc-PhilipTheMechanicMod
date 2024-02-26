using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic
{
    public static class DialogueRegistration
    {
        public static void LoadAll()
        {
            LoadCombatShouts();
            LoadShopConversations();
            LoadOutsideOfCombatConversations();
        }

        private static void LoadCombatShouts()
        {
            // 
            // Simple shouts
            //

            RegisterSimplePhilipShout(StandardShoutHooks.Relevance3.ArtifactHardmode, "squint", true);
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance7.EnemyHasWeakness, "classy");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance7.ThatsALotOfDamageToUs, "squint");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance8.Duo_AboutToDieAndLoop, "gameover");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance7.WeDidOverFiveDamage, "maniacal");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance6.WeAreCorroded, "unhappy");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance5.TheyGotCorroded, "squint");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance6.HandOnlyHasTrashCards, "sheepish");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance8.EmptyHandWithEnergy, "unhappy");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance7.ArtifactTiderunner, "excited");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance6.WeGotHurtButNotTooBad, "classy");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance5.ArtifactJetThrusters, "excited");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance4.BlockedALotOfAttacksWithArmor, "classy");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance2.WeJustOverheated, "squint");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance7.ManyTurns, "unhappy");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance7.EnemyHasBrittle, "neutral");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance6.WeDidOverThreeDamage, "neutral");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance6.WeGotShotButTookNoDamage, "neutral");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance6.WeDontOverlapWithEnemyAtAll, "neutral");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance8.JustHitGeneric, "neutral");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance7.ArtifactTridimensionalCockpit, "neutral");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance5.ArtifactNanofiberHull1, "neutral");
            RegisterSimplePhilipShout(StandardShoutHooks.Relevance4.ArtifactNanofiberHull2, "neutral");

            RegisterSimplePhilipShout
            (
                ("Custom_NewLoop", new StoryNode()
                {
                    zones = ["zone_first"],
                    oncePerRun = true,
                    oncePerCombatTags = ["Philip_AimlessChatting"]
                }),
                looptag: "maniacal"
            );

            RegisterSimplePhilipShout
            (
                ("Custom_GoingToOverheat", new StoryNode()
                {
                    oncePerRun = true,
                    goingToOverheat = true,
                })
            );

            RegisterSimplePhilipShout
            (
                ("Custom_DidntOverheat", new StoryNode()
                {
                    oncePerRun = true,
                    wasGoingToOverheatButStopped = true,
                    requiredScenes = ["Custom_GoingToOverheat"],
                }),
                looptag: "classy"
            );

            //
            // Register modified card shouts
            //

            RegisterModifiedCardShout(
                "shard",
                characterText: "The power of friendship!",
                characterLooptag: "paws",
                philipText: "Get 'em Books!!",
                philiploopTag: "excited");
            RegisterModifiedCardShout(
                "riggs",
                characterText: "Woah! That was cool!",
                philipText: "There's more where that came from!");
            RegisterModifiedCardShout(
                "hacker",
                characterText: "Hey, how'd you bypass my security?",
                characterLooptag: "squint",
                philipText: "I have my ways.",
                philiploopTag: "classy");
            RegisterModifiedCardShout(
                "dizzy",
                characterText: "How'd you do that?",
                characterLooptag: "serious",
                philipText: "Hopes, dreams, and a screwdriver.");
            RegisterModifiedCardShout(
                "peri",
                characterText: "Good work, Philip.");
            RegisterModifiedCardShout(
                "comp",
                characterText: "I don't remember giving you write privlidges...",
                characterLooptag: "squint",
                philipText: "Please don't eject me.",
                philiploopTag: "sheepish");

            //
            // Register combat multis
            //

            DB.story.all[$"FreeRedraw"] = new StoryNode()
            {
                type = NodeType.@combat,
                priority = false,
                oncePerRun = true,
                lookup = new() { "JustDidRedraw" },
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key() },
                hasArtifacts = new() { "HotChocolate" },
                lines = new()
                {
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Ahhh, hot chocolate makes reorganizing so much easier.",
                        loopTag = "hotchocolate"
                    },
                }
            };

            //
            // Insert into shout multis
            //

            (DB.story.all["ShopKeepBattleInsult"].lines[0] as SaySwitch)!.lines.Add(new RandallMod.CustomSay()
            {
                who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", "ShopKeepBattleInsult:Philip"]).Localize("en")!,
                loopTag = "whatisthat"
            });

            (DB.story.all["CrabFacts1_Multi_0"].lines[1] as SaySwitch)!.lines.Add(new RandallMod.CustomSay()
            {
                who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", "CrabFacts1_Multi_0:Philip"]).Localize("en")!
            });
            (DB.story.all["CrabFacts2_Multi_0"].lines[1] as SaySwitch)!.lines.Add(new RandallMod.CustomSay()
            {
                who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", "CrabFacts2_Multi_0:Philip"]).Localize("en")!,
                loopTag = "squint"
            });
            (DB.story.all["CrabFactsAreOverNow_Multi_0"].lines[1] as SaySwitch)!.lines.Add(new RandallMod.CustomSay()
            {
                who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", "CrabFactsAreOverNow_Multi_0:Philip"]).Localize("en")!,
                loopTag = "classy"
            });
            (DB.story.all["Soggins_Missile_Shout_1"].lines[1] as SaySwitch)!.lines.Add(new RandallMod.CustomSay()
            {
                who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", "Soggins_Missile_Shout_1:Philip"]).Localize("en")!,
                loopTag = "laugh"
            });
            (DB.story.all["SogginsEscapeIntent_1"].lines[1] as SaySwitch)!.lines.Add(new RandallMod.CustomSay()
            {
                who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", "SogginsEscapeIntent_1:Philip"]).Localize("en")!,
            });
            (DB.story.all["WeJustGainedHeatAndDrakeIsHere_Multi_0"].lines[0] as SaySwitch)!.lines.Add(new RandallMod.CustomSay()
            {
                who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", "WeJustGainedHeatAndDrakeIsHere_Multi_0:Philip"]).Localize("en")!,
                loopTag = "unhappy"
            });

            //
            // Register Midcombat Conversations
            //

            DB.story.all["Philip.ShardKickTheirButt"] = new StoryNode()
            {
                type = NodeType.@combat,
                priority = false,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), "shard" },
                lines = new() {
                    new RandallMod.CustomSay()
                    {
                        who = "shard",
                        Text = "KICK THEIR BUTT!",
                        loopTag = "squint",
                    },
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Yeah, you tell 'em Books!!",
                        loopTag = "excited"
                    },
                }
            };

            DB.story.all["Philip.SelfPropellingCannons"] = new StoryNode()
            {
                type = NodeType.@combat,
                priority = false,
                oncePerRun = true,
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), "goat" },
                hasArtifacts = new() { "clay.PhilipTheMechanic.Artifacts.SelfPropellingCannons" },
                lines = new()
                {
                    new RandallMod.CustomSay()
                    {
                        who = "goat",
                        Text = "Hey Philip, you DID remember to secure the cannon struts, right?",
                        loopTag = "squint",
                    },
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Secure the...",
                        delay = 1,
                        loopTag = "squint"
                    },
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Oh. Uh oh.",
                        delay = 2,
                        loopTag = "gameover"
                    },
                }
            };

            DB.story.all["Philip.ObjectionableConsoleRemoval"] = new StoryNode()
            {
                type = NodeType.@combat,
                priority = false,
                oncePerRun = true,
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key() },
                oncePerCombatTags = ["Philip_AimlessChatting"],
                lines = new()
                {
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Hey, you guys don't need monitors on your command consoles, right?",
                    },
                    new SaySwitch()
                    {
                        lines = new()
                        {
                            new RandallMod.CustomSay()
                            {
                                who = "peri",
                                Text = "Yes.",
                                loopTag = "squint",
                            },
                            new RandallMod.CustomSay()
                            {
                                who = "shard",
                                Text = "I don't think so!",
                            },
                            new RandallMod.CustomSay()
                            {
                                who = "hacker",
                                Text = "Don't touch my console.",
                                loopTag = "squint",
                            },
                            new RandallMod.CustomSay()
                            {
                                who = "goat",
                                Text = "Please don't take my monitor.",
                                loopTag = "squint",
                            },
                            new RandallMod.CustomSay()
                            {
                                who = "dizzy",
                                Text = "Nah!",
                            },
                            new RandallMod.CustomSay()
                            {
                                who = "riggs",
                                Text = "Ah... yeah I do.",
                                loopTag = "squint",
                            },
                        }
                    }
                }
            };
        }
        private static void RegisterSimplePhilipShout((string, StoryNode) node, string looptag = "neutral", bool isAimlessChatting = false)
        {
            var storyNode = Mutil.DeepCopy(node.Item2);
            storyNode.oncePerCombat = true; // to make him not be so chatty
            storyNode.allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key() };
            storyNode.lines = new()
            {
                new RandallMod.CustomSay()
                {
                    who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                    Text = ((JsonLocalizationProvider)ModEntry.Instance.AnyLocalizations).Bind(["dialogue", "shout", node.Item1]).Localize("en")!,
                    loopTag = looptag
                }
            };

            if (isAimlessChatting)
            {
                storyNode.oncePerCombatTags = storyNode.oncePerCombatTags ?? new();
                storyNode.oncePerCombatTags.Add("Philip_AimlessChatting");
            }

            DB.story.all[$"{node.Item1}_{ModEntry.Instance.PhilipDeck.Deck.Key()}"] = storyNode;
        }

        private static void RegisterModifiedCardShout(string character, string? characterText = null, string? characterLooptag = null, string? philipText = null, string? philiploopTag = null)
        {
            List<Instruction> instructions = new();
            if (characterText != null)
                instructions.Add(new RandallMod.CustomSay()
                {
                    who = character,
                    Text = characterText,
                    loopTag = characterLooptag,
                });

            if (philipText != null)
                instructions.Add(new RandallMod.CustomSay()
                {
                    who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                    Text = philipText,
                    loopTag = philiploopTag
                });

            DB.story.all[$"{character}Card_ModifiedByPhilip_Dialogue"] = new StoryNode()
            {
                type = NodeType.@combat,
                priority = false,
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), character },
                oncePerRun = true,
                lookup = new() { $"{character}Card_ModifiedByPhilip" },
                lines = instructions
            };
        }

        private static void LoadShopConversations()
        {
            DB.story.all[$"Philip.ShopNanobots"] = new StoryNode()
            {
                type = NodeType.@event,
                lookup = new HashSet<string>() { "shopBefore" },
                bg = "BGShop",
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key() },
                lines = new()
                {
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "Hey Philip, you don't have nanobots on you again, do you?"
                    },
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Ahahahaha.... no?",
                        loopTag = "sheepish"
                    },
                    new Jump()
                    {
                        key = "NewShop"
                    }
                }
            };

            DB.story.all[$"Philip.ShopIllegalParts"] = new StoryNode()
            {
                type = NodeType.@event,
                lookup = new HashSet<string>() { "shopBefore" },
                bg = "BGShop",
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key() },
                lines = new()
                {
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Hey Cleo! Got any parts for me?",
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "Nothing I can sell you."
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = "nerd",
                        Text = "Legally."
                    },
                    new Jump()
                    {
                        key = "NewShop"
                    }
                }
            };
        }



        private static void LoadOutsideOfCombatConversations()
        {
            DB.story.all[$"Philip.IsaacHardhat"] = new StoryNode()
            {
                type = NodeType.@event,
                oncePerRun = true,
                lookup = new HashSet<string>() { "after_any" },
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), "goat" },
                lines = new()
                    {
                    new RandallMod.CustomSay()
                    {
                        who = "goat",
                        Text = "Hey Philip, how come I never see you wearing a hardhat? Shouldn't that be required, for safety?"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Nothing falls when you've got the gravity off!",
                    },
                    new RandallMod.CustomSay()
                    {
                        who = "goat",
                        Text = "Ah... that makes sense.",
                        loopTag = "shy"
                    },
                }
            };

            DB.story.all[$"Philip.IsaacCannonStruts"] = new StoryNode()
            {
                type = NodeType.@event,
                oncePerRun = true,
                lookup = new HashSet<string>() { "after_any" },
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), "goat" },
                lines = new()
                    {
                    new RandallMod.CustomSay()
                    {
                        who = "goat",
                        Text = "Hey, are you sure that engine will hold?"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Certain!",
                        loopTag = "classy"
                    },
                    new RandallMod.CustomSay()
                    {
                        who = "goat",
                        Text = "Because without a strut there, and some plating there... it'll rip itself out of the ship.",
                        loopTag = "squint"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "... oh... I see.",
                        loopTag = "whatisthat"
                    },
                }
            };

            DB.story.all[$"Philip.PeriEfficiency"] = new StoryNode()
            {
                type = NodeType.@event,
                oncePerRun = true,
                lookup = new HashSet<string>() { "after_any" },
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), "peri" },
                lines = new()
                    {
                    new RandallMod.CustomSay()
                    {
                        who = "peri",
                        Text = "Philip, this cannon will work at higher efficiency, right?"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Yep, I'm 80% confident it won't explode this time."
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Wait I misunderstood your question.",
                        loopTag = "squint"
                    },
                    new RandallMod.CustomSay()
                    {
                        who = "peri",
                        Text = "...",
                        loopTag = "squint"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "My answer's still the same though.",
                        loopTag = "classy"
                    },
                }
            };

            DB.story.all[$"Philip.DizzyExplosion"] = new StoryNode()
            {
                type = NodeType.@event,
                oncePerRun = true,
                lookup = new HashSet<string>() { "after_any" },
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), "dizzy" },
                lines = new()
                    {
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Hey Dizzy! Want to see a phosphorus explosion?"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = "dizzy",
                        Text = "Do I ever!"
                    },
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "... from behind the observation window.",
                        loopTag = "squint"
                    },
                    new RandallMod.CustomSay()
                    {
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "I'm in here because I'm certified to have my fur burnt off.",
                        loopTag = "maniacal"
                    },
                }
            };

            DB.story.all[$"Philip.BooksSticker"] = new StoryNode()
            {
                type = NodeType.@event,
                oncePerRun = true,
                lookup = new HashSet<string>() { "after_any" },
                allPresent = new() { ModEntry.Instance.PhilipDeck.Deck.Key(), "shard" },
                lines = new()
                    {
                    new RandallMod.CustomSay()
                    {
                        who = "shard",
                        Text = "Hi Mr. Philip! I heard you have a sticker collection!"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Sure do!",
                    },
                    new RandallMod.CustomSay()
                    {
                        who = "shard",
                        Text = "Can I have one?"
                    },
                    new RandallMod.CustomSay()
                    {
                        flipped = true,
                        who = ModEntry.Instance.PhilipDeck.Deck.Key(),
                        Text = "Sure! Here take... this one.",
                    },
                    new RandallMod.CustomSay()
                    {
                        who = "shard",
                        Text = "Thank you Mr. Philip!"
                    },
                }
            };
        }
    }
}
