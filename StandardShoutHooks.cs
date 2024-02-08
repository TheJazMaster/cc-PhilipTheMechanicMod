using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clay.PhilipTheMechanic
{
    public static class StandardShoutHooks
    {
        private static NodeType NodeTypeCombat = Enum.Parse<NodeType>("combat");
        private static Dictionary<string, Deck> NameToDeck_Cache = new() {};
        private static Deck NameToDeck(string name) 
        { 
            if (!NameToDeck_Cache.ContainsKey(name)) NameToDeck_Cache.Add(name, Enum.Parse<Deck>(name));
            return NameToDeck_Cache[name];
        }
        private static Dictionary<string, Status> NameToStatus_Cache = new() {};
        private static Status NameToStatus(string name) 
        { 
            if (!NameToStatus_Cache.ContainsKey(name)) NameToStatus_Cache.Add(name, Enum.Parse<Status>(name));
            return NameToStatus_Cache[name];
        }
        private static HashSet<Status> NamesToStatuses(HashSet<string> names)
        {
            HashSet<Status> result = new HashSet<Status>();
            foreach (string name in names) 
            {
                result.Add(NameToStatus(name));
            }
            return result;
        }

        // All 8 crewmates comment on these events. Highly reccomended to write dialogue for these.
        public class Relevance8
        {
            // should not be referenced as a new shout, modify game's existing shout sayswitch for BanditThreats_Multi_0 specifically
            public static StoryNode BanditThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "bandit" },
                maxTurnsThisCombat = 1,
                priority = true,
                oncePerCombatTags = new() { "BanditThreats" }
            };
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static StoryNode CrabFacts1 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "crab" },
                priority = true,
                minTurnsThisCombat = 2,
                maxTurnsThisCombat = 2
            };
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static StoryNode CrabFacts2 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "crab" },
                priority = true,
                minTurnsThisCombat = 3,
                maxTurnsThisCombat = 3
            };
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static StoryNode CrabFactsAreOverNow => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "crab" },
                priority = true,
                minTurnsThisCombat = 4,
                maxTurnsThisCombat = 4
            };
            public static StoryNode Duo_AboutToDieAndLoop => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = new() { "aboutToDie" },
                oncePerRun = true,
            };
            public static StoryNode EmptyHandWithEnergy => new StoryNode()
            {
                type = NodeTypeCombat,
                handEmpty = true,
                minEnergy = 1,
               
            };
            public static StoryNode JustHitGeneric => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
            };
            public static StoryNode OverheatDrakeFix => new StoryNode()
            {
                type = NodeTypeCombat,
                wasGoingToOverheatButStopped = true,
                whoDidThat = NameToDeck("eunice"),
                allPresent = new() { "eunice" },
                oncePerCombatTags = new() { "OverheatDrakeFix" }
            };
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static StoryNode ShopKeepBattleInsult => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                allPresent = new() { "nerd" },
                oncePerRunTags = new() { "ShopkeepAboutToDestroyYou" },
                enemyIntent = "shopkeepAttack"
            };
            public static StoryNode VeryManyTurns => new StoryNode()
            {
                type = NodeTypeCombat,
                minTurnsThisCombat = 20,
                oncePerCombatTags = new() { "veryManyTurns" },
                oncePerRun = true,
                turnStart = true,
               
            };
        }

        public class Relevance7
        {
            public static StoryNode ArtifactGeminiCore => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "GeminiCore" },
                oncePerRunTags = new() { "GeminiCore" },
               
            };
            public static StoryNode ArtifactTiderunner => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "TideRunner" },
                oncePerCombatTags = new() { "TideRunner" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactTridimensionalCockpit => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerCombatTags = new() { "TridimensionalCockpit" },
                hasArtifacts = new() { "TridimensionalCockpit" },
                oncePerRun = true,
               
            };
            public static StoryNode EnemyHasBrittle => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyHasBrittlePart = true,
                oncePerRunTags = new() { "yelledAboutBrittle" },
               
            };
            public static StoryNode EnemyHasWeakness => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyHasWeakPart = true,
                oncePerRunTags = new() { "yelledAboutWeakness" },
               
            };
            public static StoryNode JustPlayedADraculaCard => new StoryNode()
            {
                type = NodeTypeCombat,
                whoDidThat = NameToDeck("dracula"),
                oncePerRun = true,
               
            };
            public static StoryNode ManyTurns => new StoryNode()
            {
                type = NodeTypeCombat,
                minTurnsThisCombat = 9,
                oncePerCombatTags = new() { "manyTurns" },
                oncePerRun = true,
                turnStart = true,
               
            };
            public static StoryNode OneHitPointThisIsFine => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerCombatTags = new() { "aboutToDie" },
                oncePerRun = true,
                enemyShotJustHit = true,
                maxHull = 1,
               
            };
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static StoryNode SogginsEscapeIntent_1 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                enemyIntent = "sogginsEscapeIntent",
                turnStart = true,
                priority = true,
                oncePerRun = true,
                specialFight = "sogginsMissileEvent"
            };
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static StoryNode Soggins_Missile_Shout_1 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true
            };
            public static StoryNode ThatsALotOfDamageToUs => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                minDamageDealtToPlayerThisTurn = 3,
               
            };
            public static StoryNode WeDidOverFiveDamage => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 6,
               
            };
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static StoryNode WeJustGainedHeatAndDrakeIsHere => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "heat" }),
                allPresent = new() { "eunice" },
                oncePerCombatTags = new() { "DrakeCanYouDoSomethingAboutTheHeatPlease" }
            };
        }

        public class Relevance6
        {
            public static StoryNode ArtifactRecalibrator => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustMissed = true,
                hasArtifacts = new() { "Recalibrator" },
               
            };
            public static StoryNode BooksWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingBooks" }),
                priority = true,
                oncePerCombatTags = new() { "booksWentMissing" },
                oncePerRun = true,
               
            };
            public static StoryNode CatWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingCat" }),
                priority = true,
                oncePerCombatTags = new() { "CatWentMissing" },
                oncePerRun = true,
               
            };
            public static StoryNode HandOnlyHasTrashCards => new StoryNode()
            {
                type = NodeTypeCombat,
                handFullOfTrash = true,
                oncePerCombatTags = new() { "handOnlyHasTrashCards" },
                oncePerRun = true,
               
            };
            public static StoryNode OverheatGeneric => new StoryNode()
            {
                type = NodeTypeCombat,
                goingToOverheat = true,
                oncePerCombatTags = new() { "OverheatGeneric" },
               
            };
            public static StoryNode RiggsWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingRiggs" }),
                priority = true,
                oncePerCombatTags = new() { "riggsWentMissing" },
                oncePerRun = true,
               
            };
            public static StoryNode ThatsALotOfDamageToThem => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 10,
               
            };
            public static StoryNode TheyHaveAutoDodgeLeft => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnEnemyStatuses = NamesToStatuses(new() { "autododgeLeft" }),
                oncePerCombatTags = new() { "aboutAutododge" },
                oncePerRun = true,
               
            };
            public static StoryNode TheyHaveAutoDodgeRight => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnEnemyStatuses = NamesToStatuses(new() { "autododgeRight" }),
                oncePerCombatTags = new() { "aboutAutododge" },
                oncePerRun = true,
               
            };
            public static StoryNode WeAreCorroded => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "corrode" }),
                oncePerRun = true,
               
            };
            public static StoryNode WeDidOverThreeDamage => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 4,
               
            };
            public static StoryNode WeDontOverlapWithEnemyAtAll => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                shipsDontOverlapAtAll = true,
                oncePerCombatTags = new() { "NoOverlapBetweenShips" },
                oncePerRun = true,
                nonePresent = new() { "crab", "scrap" },
               
            };
            public static StoryNode WeGotHurtButNotTooBad => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                minDamageDealtToPlayerThisTurn = 1,
                maxDamageDealtToPlayerThisTurn = 1,
               
            };
            public static StoryNode WeGotShotButTookNoDamage => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                maxDamageDealtToPlayerThisTurn = 0,
               
            };
            public static StoryNode WeMissedOopsie => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustMissed = true,
                oncePerCombat = true,
                doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
               
            };
        }

        public class Relevance5
        {
            public static StoryNode ArtifactCockpitTargetIsRelevant => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                enemyHasPart = "cockpit",
                hasArtifacts = new() { "CockpitTarget" },
               
            };
            public static StoryNode ArtifactCrosslink => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "Crosslink" },
                lookup = new() { "CrosslinkTrigger" },
                oncePerRun = true,
                oncePerCombatTags = new() { "CrosslinkTriggerTag" },
               
            };
            public static StoryNode ArtifactFractureDetection => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "FractureDetection" },
                oncePerCombatTags = new() { "FractureDetectionBarks" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactJetThrusters => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "JetThrusters" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactNanofiberHull1 => new StoryNode()
            {
                type = NodeTypeCombat,
                minDamageDealtToPlayerThisTurn = 1,
                maxDamageDealtToPlayerThisTurn = 1,
                hasArtifacts = new() { "NanofiberHull" },
                oncePerRunTags = new() { "NanofiberHull" },
               
            };
            public static StoryNode BlockedAnEnemyAttackWithArmor => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 1,
                oncePerCombatTags = new() { "WowArmorISPrettyCoolHuh" },
                oncePerRun = true,
               
            };
            public static StoryNode DizzyWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingDizzy" }),
                priority = true,
                oncePerCombatTags = new() { "dizzyWentMissing" },
               
            };
            public static StoryNode DrakeWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingDrake" }),
                priority = true,
                oncePerCombatTags = new() { "drakeWentMissing" },
                oncePerRun = true,
               
            };
            public static StoryNode EnemyArmorHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageBlockedByEnemyArmorThisTurn = 1,
                oncePerCombat = true,
                oncePerRun = true,
               
            };
            public static StoryNode IsaacWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingIsaac" }),
                priority = true,
                oncePerCombatTags = new() { "isaacWentMissing" },
                oncePerRun = true,
               
            };
            public static StoryNode LookOutMissile => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                once = true,
                oncePerRunTags = new() { "goodMissileAdvice" },
                anyDronesHostile = new() { "missile_normal", "missile_heavy", "missile_corrode", "missile_seeker", "missile_breacher" },
               
            };
            public static StoryNode MaxWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingMax" }),
                priority = true,
                oncePerCombatTags = new() { "maxWentMissing" },
                oncePerRun = true,
               
            };
            public static StoryNode OverheatDrakesFault => new StoryNode()
            {
                type = NodeTypeCombat,
                goingToOverheat = true,
                whoDidThat = NameToDeck("eunice"),
                allPresent = new() { "eunice" },
                oncePerCombatTags = new() { "OverheatDrakesFault" }
            };
            public static StoryNode PeriWentMissing => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "missingPeri" }),
                priority = true,
                oncePerCombatTags = new() { "periWentMissing" },
                oncePerRun = true,
               
            };
            public static StoryNode SpikeGetsChatty => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "oxygenguy" },
                oncePerCombatTags = new() { "SpikeGoAway" }
            };
            public static StoryNode StrafeHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                playerShotWasFromStrafe = true,
                oncePerCombat = true,
               
            };
            public static StoryNode TheyGotCorroded => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnEnemyStatuses = NamesToStatuses(new() { "corrode" }),
                oncePerRun = true,
               
            };
        }

        public class Relevance4
        {
            public static StoryNode ArtifactAresCannon => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "AresCannon" },
                oncePerRunTags = new() { "AresCannon" },
               
            };
            public static StoryNode ArtifactBrokenGlasses => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                hasArtifacts = new() { "BrokenGlasses" },
               
            };
            public static StoryNode ArtifactEnergyRefund => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "EnergyRefund" },
                minCostOfCardJustPlayed = 3,
                oncePerCombatTags = new() { "EnergyRefund" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactNanofiberHull2 => new StoryNode()
            {
                type = NodeTypeCombat,
                minDamageDealtToPlayerThisTurn = 2,
                hasArtifacts = new() { "NanofiberHull" },
                oncePerRunTags = new() { "NanofiberHull2" },
               
            };
            public static StoryNode ArtifactOverclockedGeneratorSeenMaxMemory3 => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "OverclockedGenerator" },
                priority = true,
                oncePerRunTags = new() { "OverclockedGeneratorTag" },
                lookup = new() { "OverclockedGeneratorTrigger" },
                requiredScenes = new() { "Hacker_Memory_3" },
               
            };
            public static StoryNode ArtifactShieldPrepIsGone => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = new() { "ShieldPrepIsGoneYouFool" },
                doesNotHaveArtifacts = new() { "ShieldPrep", "WarpMastery" },
               
            };
            public static StoryNode ArtifactWarpMastery => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = new() { "WarpMastery" },
                hasArtifacts = new() { "WarpMastery" },
               
            };
            public static StoryNode BatboyKeepsTalking => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "batboy" },
                oncePerCombatTags = new() { "batboyshutupyouBigNERD" }
            };
            public static StoryNode BlockedALotOfAttacksWithArmor => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 3,
                oncePerCombatTags = new() { "YowzaThatWasALOTofArmorBlock" },
                oncePerRun = true,
               
            };
            public static StoryNode JustPlayedASashaCard => new StoryNode()
            {
                type = NodeTypeCombat,
                whoDidThat = NameToDeck("sasha"),
                oncePerRunTags = new() { "usedASashaCard" }
            };
            public static StoryNode SogginsEscapeIntent_3 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                enemyIntent = "sogginsEscapeIntentPlease",
                turnStart = true,
                priority = true,
                oncePerRun = true,
                specialFight = "sogginsMissileEvent",
                requiredScenes = new() { "SogginsEscapeIntent_2" }
            };
            public static StoryNode WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                shipsDontOverlapAtAll = true,
                oncePerCombatTags = new() { "NoOverlapBetweenShipsSeeker" },
                anyDronesHostile = new() { "missile_seeker" },
                nonePresent = new() { "crab" },
               
            };
        }

        public class Relevance3
        {
            public static StoryNode ArtifactArmoredBay => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 1,
                oncePerCombatTags = new() { "ArmoredBae" },
                hasArtifacts = new() { "ArmoredBay" },
               
            };
            public static StoryNode ArtifactCockpitTargetIsNotRelevant => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                enemyDoesNotHavePart = "cockpit",
                hasArtifacts = new() { "CockpitTarget" },
               
            };
            public static StoryNode ArtifactDizzyBoost => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                whoDidThat = NameToDeck("dizzy"),
                hasArtifacts = new() { "DizzyBoost" },
                oncePerCombat = true,
                allPresent = new() { "dizzy" }
            };
            public static StoryNode ArtifactEnergyPrep => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "EnergyPrep" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactGeminiCoreBooster => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "GeminiCoreBooster" },
                oncePerRunTags = new() { "GeminiCoreBooster" },
               
            };
            public static StoryNode ArtifactHardmode => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                hasArtifacts = new() { "HARDMODE" },
                oncePerRunTags = new() { "HARDMODE" },
                once = true,
               
            };
            public static StoryNode ArtifactHullPlatingWhenShot => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                oncePerRun = true,
                hasArtifacts = new() { "HullPlating" },
               
            };
            public static StoryNode ArtifactIonConverter => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "IonConverter" },
                lookup = new() { "IonConverterTrigger" },
                oncePerRun = true,
                priority = true,
                oncePerCombatTags = new() { "IonConverterTag" },
               
            };
            public static StoryNode ArtifactOverclockedGenerator => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "OverclockedGenerator" },
                oncePerRunTags = new() { "OverclockedGeneratorTag" },
                lookup = new() { "OverclockedGeneratorTrigger" },
               
            };
            public static StoryNode ArtifactPiercer => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "Piercer" },
                oncePerCombatTags = new() { "PiercerShouts" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactPowerDiversionMadeDizzyAttackFail => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                maxDamageDealtToEnemyThisAction = 0,
                whoDidThat = NameToDeck("dizzy"),
                hasArtifacts = new() { "PowerDiversion" },
                allPresent = new() { "dizzy", "peri" }
            };
            public static StoryNode ArtifactQuickDraw => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "Quickdraw" },
               
            };
            public static StoryNode ArtifactSimplicity => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRunTags = new() { "SimplicityShouts" },
                hasArtifacts = new() { "Simplicity" },
               
            };
            public static StoryNode BooksJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = NameToDeck("shard"),
               
                oncePerCombatTags = new() { "BooksShotThatGuy" }
            };
            public static StoryNode CATJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = NameToDeck("colorless"),
               
            };
            public static StoryNode CheapCardPlayed => new StoryNode()
            {
                type = NodeTypeCombat,
                maxCostOfCardJustPlayed = 0,
                oncePerCombatTags = new() { "CheapCardPlayed" },
                oncePerRun = true,
               
            };
            public static StoryNode DizzyJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = NameToDeck("dizzy"),
               
            };
            public static StoryNode EvadeLastsBetweenTurns => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                once = true,
                oncePerCombatTags = new() { "goodEvadeAdvice" },
                lastTurnPlayerStatuses = NamesToStatuses(new() { "evade" }),
                minRuns = 1,
               
            };
            public static StoryNode ExpensiveCardPlayed => new StoryNode()
            {
                type = NodeTypeCombat,
                minCostOfCardJustPlayed = 4,
                oncePerCombatTags = new() { "ExpensiveCardPlayed" },
                oncePerRun = true,
               
            };
            public static StoryNode HackerJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = NameToDeck("hacker"),
               
            };
            public static StoryNode HandOnlyHasUnplayableCards => new StoryNode()
            {
                type = NodeTypeCombat,
                handFullOfUnplayableCards = true,
                oncePerCombatTags = new() { "handFullOfUnplayableCards" },
                oncePerRun = true,
               
            };
            public static StoryNode JustPlayedASogginsCard => new StoryNode()
            {
                type = NodeTypeCombat,
                whoDidThat = NameToDeck("soggins"),
                oncePerRun = true,
               
            };
            public static StoryNode PeriJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                oncePerRun = true,
                oncePerCombatTags = new() { "PeriHitEmYo" },
                whoDidThat = NameToDeck("peri"),
               
            };
            public static StoryNode PlayedManyCards => new StoryNode()
            {
                type = NodeTypeCombat,
                handEmpty = true,
                minCardsPlayedThisTurn = 6,
               
            };
            public static StoryNode StardogGetsChatty => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "wolf" },
                oncePerCombatTags = new() { "StardogLeaveUsAlone" }
            };
            public static StoryNode TookDamageHave2HP => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                minHull = 2,
                maxHull = 2,
                oncePerRunTags = new() { "TookDamageHave2HP" },
               
            };
            public static StoryNode TookZeroDamageAtLowHealth => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                maxDamageDealtToPlayerThisTurn = 0,
                maxHull = 2,
               
            };
            public static StoryNode WizardGeneralShouts => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "wizard" },
                enemyIntent = "wizardMagic"
            };
        }

        public class Relevance2
        {
            public static StoryNode ArtifactAresCannonV2 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "AresCannonV2" },
                oncePerRunTags = new() { "AresCannonV2" },
               
            };
            public static StoryNode ArtifactDirtyEngines => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                hasArtifacts = new() { "DirtyEngines" },
               
            };
            public static StoryNode ArtifactFlywheel => new StoryNode()
            {
                type = NodeTypeCombat,
                hasArtifacts = new() { "Flywheel" },
                oncePerRunTags = new() { "Flywheel" },
               
            };
            public static StoryNode ArtifactGravelRecyclerGeode => new StoryNode()
            {
                type = NodeTypeCombat,
                anyDrones = new() { "asteroidShard" },
                hasArtifacts = new() { "GravelRecycler" },
                oncePerCombatTags = new() { "GravelRecyclerGeode" },
               
            };
            public static StoryNode ArtifactGravelRecycler => new StoryNode()
            {
                type = NodeTypeCombat,
                anyDrones = new() { "asteroid" },
                hasArtifacts = new() { "GravelRecycler" },
                oncePerCombatTags = new() { "GravelRecycler" },
               
            };
            public static StoryNode ArtifactJetThrustersNoRiggs => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = new() { "JetThrusters" },
                nonePresent = new() { "riggs" },
                oncePerRunTags = new() { "OncePerRunThrusterJokesAboutRiggsICanMakeTheseTagsStupidlyLongIfIWant" },
               
            };
            public static StoryNode ArtifactNanofiberHullHealBooster => new StoryNode()
            {
                type = NodeTypeCombat,
                minDamageDealtToPlayerThisTurn = 2,
                maxDamageDealtToPlayerThisTurn = 2,
                hasArtifacts = new() { "NanofiberHull", "HealBooster" },
                oncePerRunTags = new() { "NanofiberHull3" },
               
            };
            public static StoryNode CATsummonedIsaacCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonIsaac" },
                oncePerCombatTags = new() { "summonIsaacTag" },
                oncePerRun = true,
            };
            public static StoryNode CATsummonedPeriCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonPeri" },
                oncePerCombatTags = new() { "summonPeriTag" },
                oncePerRun = true,
               
            };
            public static StoryNode CATsummonedRiggsCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonRiggs" },
                oncePerCombatTags = new() { "summonRiggsTag" },
                oncePerRun = true,
               
            };
            public static StoryNode Crystal_1_2 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "crystal" },
                requiredScenes = new() { "Crystal_1", "Crystal_1_1" },
                excludedScenes = new() { "Crystal_2" }
            };
            public static StoryNode EnemyArmorHitLots => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageBlockedByEnemyArmorThisTurn = 3,
                oncePerCombat = true,
                oncePerRun = true,
               
            };
            public static StoryNode EnemyArmorPierced => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                playerJustPiercedEnemyArmor = true,
                oncePerCombatTags = new() { "EnemyArmorPierced" },
                oncePerRun = true,
               
            };
            public static StoryNode EuniceJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = NameToDeck("eunice"),
               
            };
            public static StoryNode GoatJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = NameToDeck("goat"),
               
            };
            public static StoryNode JustPlayedAToothCard => new StoryNode()
            {
                type = NodeTypeCombat,
                whoDidThat = NameToDeck("tooth"),
                oncePerRunTags = new() { "usedAToothCard" },
               
            };
            public static StoryNode ManyFlips => new StoryNode()
            {
                type = NodeTypeCombat,
                minTimesYouFlippedACardThisTurn = 4,
                oncePerCombat = true,
               
            };
            public static StoryNode OverheatCatFix => new StoryNode()
            {
                type = NodeTypeCombat,
                wasGoingToOverheatButStopped = true,
                whoDidThat = NameToDeck("colorless"),
               
                oncePerCombatTags = new() { "OverheatCatFix" }
            };
            public static StoryNode PeriJustHitAndDidBigDamage => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 3,
                whoDidThat = NameToDeck("peri"),
               
            };
            public static StoryNode RiggsJustHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = NameToDeck("riggs"),
               
            };
            public static StoryNode RiggsSeesDrakesCoolCard => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                lookup = new() { "drakesCoolCard" },
                once = true,
                allPresent = new() { "eunice", "riggs" }
            };
            public static StoryNode StrafeMissedGood => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustMissed = true,
                playerShotWasFromStrafe = true,
                hasArtifacts = new() { "Recalibrator", "GrazerBeam" },
                oncePerCombat = true,
               
            };
            public static StoryNode TheCobalt_1_1 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "theCobalt" },
                requiredScenes = new() { "TheCobalt_1" },
                excludedScenes = new() { "TheCobalt_2" }
            };
            public static StoryNode TheCobalt_1_2 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "theCobalt" },
                requiredScenes = new() { "TheCobalt_1", "TheCobalt_1_1" },
                excludedScenes = new() { "TheCobalt_2" }
            };
            public static StoryNode TheCobalt_1_3 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "theCobalt" },
                requiredScenes = new() { "TheCobalt_1", "TheCobalt_1_2" },
                excludedScenes = new() { "TheCobalt_2" }
            };
            public static StoryNode WeJustOverheated => new StoryNode()
            {
                type = NodeTypeCombat,
                justOverheated = true,
                oncePerCombatTags = new() { "WeJustOverheated" },
            };
        }

        public class Relevance1
        {
            public static StoryNode ArtifactBerserkerDrive => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 8,
                oncePerRun = true,
                hasArtifacts = new() { "BerserkerDrive" },
               
            };
            public static StoryNode ArtifactDemonThrusters => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                hasArtifacts = new() { "DemonThrusters" },
                oncePerRunTags = new() { "ArtifactDemonThrusters" },
               
            };
            public static StoryNode ArtifactHullPlatingWhenMissed => new StoryNode()
            {
                type = NodeTypeCombat,
                enemyShotJustHit = true,
                oncePerRun = true,
                hasArtifacts = new() { "HullPlating" },
               
            };
            public static StoryNode ArtifactJumperCablesUseless => new StoryNode()
            {
                type = NodeTypeCombat,
                maxTurnsThisCombat = 1,
                minHullPercent = 1,
                hasArtifacts = new() { "JumperCables" },
                oncePerRunTags = new() { "ArtifactJumperCablesUnneeded" },
               
            };
            public static StoryNode ArtifactJumperCables => new StoryNode()
            {
                type = NodeTypeCombat,
                maxTurnsThisCombat = 1,
                maxHullPercent = 0.5,
                hasArtifacts = new() { "JumperCables" },
                oncePerRunTags = new() { "ArtifactJumperCablesReady" },
               
            };
            public static StoryNode ArtifactPiercerStrafe => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                playerShotWasFromStrafe = true,
                playerShotJustHit = true,
                hasArtifacts = new() { "Piercer" },
                oncePerCombatTags = new() { "PiercerShoutsStrafe" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactPressureFuseFail => new StoryNode()
            {
                type = NodeTypeCombat,
                minHullPercent = 0.51,
                oncePerCombatTags = new() { "PressureFuseFail" },
                hasArtifacts = new() { "PressureFuse" },
               
            };
            public static StoryNode ArtifactPressureFuse => new StoryNode()
            {
                type = NodeTypeCombat,
                maxHullPercent = 0.5,
                oncePerCombatTags = new() { "PressureFuse" },
                hasArtifacts = new() { "PressureFuse" },
                oncePerRun = true,
               
            };
            public static StoryNode ArtifactRevengeDriveBig => new StoryNode()
            {
                type = NodeTypeCombat,
                minDamageDealtToPlayerThisTurn = 3,
                oncePerCombat = true,
                enemyShotJustHit = true,
                hasArtifacts = new() { "RevengeDrive" },
               
            };
            public static StoryNode ArtifactRevengeDrive => new StoryNode()
            {
                type = NodeTypeCombat,
                minDamageDealtToPlayerThisTurn = 1,
                enemyShotJustHit = true,
                hasArtifacts = new() { "RevengeDrive" },
                oncePerCombatTags = new() { "RevengeDriveShouts" },
               
            };
            public static StoryNode ArtifactSalvageArmDanger => new StoryNode()
            {
                type = NodeTypeCombat,
                anyDronesHostile = new() { "missile_normal", "missile_heavy", "missile_corrode", "missile_seeker", "missile_breacher" },
                oncePerCombatTags = new() { "SalvageMissileAdvice" },
                hasArtifacts = new() { "SalvageArm" },
               
            };
            public static StoryNode ArtifactSalvageArm => new StoryNode()
            {
                type = NodeTypeCombat,
                anyDrones = new() { "asteroid" },
                hasArtifacts = new() { "SalvageArm" },
                oncePerCombatTags = new() { "salvageAsteroidAdvice" },
               
            };
            public static StoryNode ArtifactSharpEdges => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerCombat = true,
                playerJustShuffledDiscardIntoDrawPile = true,
                hasArtifacts = new() { "SharpEdges" },
                oncePerRun = true,
               
            };
            public static StoryNode BatboyThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
               
                maxTurnsThisCombat = 1
            };
            public static StoryNode CATsummonedBooksCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonBooks" },
                oncePerCombatTags = new() { "summonBooksTag" },
                oncePerRun = true,
            };
            public static StoryNode CATsummonedCATCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonCAT" },
                oncePerCombatTags = new() { "summonCATTag" },
                oncePerRun = true,
            };
            public static StoryNode CATsummonedDizzyCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonDizzy" },
                oncePerCombatTags = new() { "summonDizzyTag" },
                oncePerRun = true,
            };
            public static StoryNode CATsummonedDrakeCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonDrake" },
                oncePerCombatTags = new() { "summonDrakeTag" },
                oncePerRun = true,
            };
            public static StoryNode CATsummonedMaxCard => new StoryNode()
            {
                type = NodeTypeCombat,
                lookup = new() { "summonMax" },
                oncePerCombatTags = new() { "summonMaxTag" },
                oncePerRun = true,
            };
            public static StoryNode Crystal_1_1 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "crystal" },
                requiredScenes = new() { "Crystal_1" },
                excludedScenes = new() { "Crystal_2" }
            };
            public static StoryNode Crystal_1_3 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "crystal" },
                requiredScenes = new() { "Crystal_1", "Crystal_1_2" },
                excludedScenes = new() { "Crystal_2" }
            };
            public static StoryNode DizzyBigHit => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 3,
                oncePerRunTags = new() { "DizzyBigHit" },
                whoDidThat = NameToDeck("dizzy"),
                allPresent = new() { "dizzy" }
            };
            public static StoryNode DrakeBot_1_1 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "drakebot", "eunice" },
                requiredScenes = new() { "DrakeBot_1" },
                excludedScenes = new() { "DrakeBot_2" }
            };
            public static StoryNode Drone_battery_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "battery"
            };
            public static StoryNode Drone_battery_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "battery"
            };
            public static StoryNode Drone_callisto_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "callisto"
            };
            public static StoryNode Drone_callisto_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "callisto"
            };
            public static StoryNode Drone_cheebo_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "cheebo"
            };
            public static StoryNode Drone_cheebo_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "cheebo"
            };
            public static StoryNode Drone_chosenone_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "chosenone"
            };
            public static StoryNode Drone_chosenone_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "chosenone"
            };
            public static StoryNode Drone_europa_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "europa"
            };
            public static StoryNode Drone_europa_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "europa"
            };
            public static StoryNode Drone_ganymede_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "ganymede"
            };
            public static StoryNode Drone_ganymede_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "ganymede"
            };
            public static StoryNode Drone_gary_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "gary"
            };
            public static StoryNode Drone_gary_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "gary"
            };
            public static StoryNode Drone_iggy_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "iggy"
            };
            public static StoryNode Drone_iggy_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "iggy"
            };
            public static StoryNode Drone_io_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "io"
            };
            public static StoryNode Drone_io_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "io"
            };
            public static StoryNode Drone_isaacjr_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "isaacjr"
            };
            public static StoryNode Drone_isaacjr_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "isaacjr"
            };
            public static StoryNode Drone_itchy_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "itchy"
            };
            public static StoryNode Drone_itchy_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "itchy"
            };
            public static StoryNode Drone_jeff_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "jeff"
            };
            public static StoryNode Drone_jeff_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "jeff"
            };
            public static StoryNode Drone_juice_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "juice"
            };
            public static StoryNode Drone_juice_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "juice"
            };
            public static StoryNode Drone_jupejr_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "jupejr"
            };
            public static StoryNode Drone_jupejr_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "jupejr"
            };
            public static StoryNode Drone_king_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "king"
            };
            public static StoryNode Drone_king_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "king"
            };
            public static StoryNode Drone_larry_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "larry"
            };
            public static StoryNode Drone_larry_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "larry"
            };
            public static StoryNode Drone_lemmy_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "lemmy"
            };
            public static StoryNode Drone_lemmy_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "lemmy"
            };
            public static StoryNode Drone_ludwig_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "ludwig"
            };
            public static StoryNode Drone_ludwig_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "ludwig"
            };
            public static StoryNode Drone_lumpy_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "lumpy"
            };
            public static StoryNode Drone_lumpy_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "lumpy"
            };
            public static StoryNode Drone_morton_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "morton"
            };
            public static StoryNode Drone_morton_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "morton"
            };
            public static StoryNode Drone_namelessone_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "namelessone"
            };
            public static StoryNode Drone_namelessone_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "namelessone"
            };
            public static StoryNode Drone_numberone_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "numberone"
            };
            public static StoryNode Drone_numberone_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "numberone"
            };
            public static StoryNode Drone_roy_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "roy"
            };
            public static StoryNode Drone_roy_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "roy"
            };
            public static StoryNode Drone_scoobert_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "scoobert"
            };
            public static StoryNode Drone_scoobert_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "scoobert"
            };
            public static StoryNode Drone_sparky_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "sparky"
            };
            public static StoryNode Drone_sparky_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "sparky"
            };
            public static StoryNode Drone_stinky_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "stinky"
            };
            public static StoryNode Drone_stinky_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "stinky"
            };
            public static StoryNode Drone_wendy_Destroyed => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneDestroyed = "wendy"
            };
            public static StoryNode Drone_wendy_Spawned => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerRun = true,
                allPresent = new() { "goat" },
                priority = true,
                lastNamedDroneSpawned = "wendy"
            };
            public static StoryNode EuniceHitOncePerRun => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustHit = true,
                oncePerRun = true,
                allPresent = new() { "eunice" }
            };
            public static StoryNode EuniceMiss => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustMissed = true,
                oncePerCombat = true,
                doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
                allPresent = new() { "eunice" }
            };
            public static StoryNode EvilRiggsWhenRiggsTriesToLeave => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "pirateBoss" },
                enemyIntent = "hardRiggsGoesAggro",
                oncePerCombat = true,
                priority = true
            };
            public static StoryNode Finale_Dizzy_2_PostShout => new StoryNode()
            {
                type = NodeTypeCombat,
                oncePerCombat = true,
                priority = true,
                specialFight = "finale",
                enemyIntent = "saveTheCrewShout",
                allPresent = new() { "comp" }
            };
            public static StoryNode IsaacHasTooMuchRockFactoryButYouHaveGravelRecycler => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                lookup = new() { "RockFactoryTooMany" },
                oncePerCombatTags = new() { "RockFactoryTooManyRecycle" },
                oncePerRun = true,
                hasArtifacts = new() { "GravelRecycler" },
                allPresent = new() { "goat" }
            };
            public static StoryNode IsaacHasTooMuchRockFactoryButYouHaveSalvageArm => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                lookup = new() { "RockFactoryTooMany" },
                oncePerCombatTags = new() { "RockFactoryTooMany" },
                oncePerRun = true,
                hasArtifacts = new() { "SalvageArm" },
                allPresent = new() { "goat" }
            };
            public static StoryNode IsaacHasTooMuchRockFactory => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                lookup = new() { "RockFactoryTooMany" },
                oncePerCombatTags = new() { "RockFactoryTooMany" },
                oncePerRun = true,
                doesNotHaveArtifacts = new() { "SalvageArm", "GravelRecycler" },
                allPresent = new() { "goat" }
            };
            public static StoryNode JustPlayedAnEphemeralCard => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                whoDidThat = NameToDeck("ephemeral"),
                oncePerRunTags = new() { "usedAnEphemeralCard" },
            };
            public static StoryNode MaxIsSuperIntoTheBandit => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "bandit", "hacker" },
                priority = true,
                oncePerCombatTags = new() { "MaxBanditCrush" }
            };
            public static StoryNode MinerOncePerFightShouts => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "miner" },
                enemyIntent = "wideBigAttack",
                oncePerCombatTags = new() { "MinerGonnaSmackYa4X" }
            };
            public static StoryNode OldSpikeChattyPostRenameGeorge => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "spike", "eunice" },
                oncePerCombatTags = new() { "OldSpikeNewName" },
                maxTurnsThisCombat = 1,
                spikeName = "george"
            };
            public static StoryNode OopsBackwardsJupiter => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                oncePerRun = true,
                oncePerCombatTags = new() { "backwardsJupiterDrone" },
                anyDronesHostile = new() { "cannonDrone" },
            };
            public static StoryNode PirateBoss_1_1 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "pirateBoss", "riggs" },
                requiredScenes = new() { "PirateBoss_1" },
                excludedScenes = new() { "PirateBoss_2" }
            };
            public static StoryNode PirateGeneralShouts => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                allPresent = new() { "pirate" },
                enemyIntent = "tauntPeri"
            };
            public static StoryNode RiggsHandCannon => new StoryNode()
            {
                type = NodeTypeCombat,
                priority = true,
                lookup = new() { "riggsHandCannon" },
                playerShotJustHit = true,
                oncePerCombatTags = new() { "HandCannon" },
                oncePerRun = true,
                allPresent = new() { "riggs" }
            };
            public static StoryNode SogginsEscapeIntent_2 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                enemyIntent = "sogginsEscapeIntentPlease",
                turnStart = true,
                priority = true,
                oncePerRun = true,
                specialFight = "sogginsMissileEvent",
                requiredScenes = new() { "SogginsEscapeIntent_1" }
            };
            public static StoryNode StoneNervous => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "stone" },
                lookup = new() { "stoneNervous" }
            };
            public static StoryNode StoneReverse => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "stone" },
                lookup = new() { "stoneReverse" }
            };
            public static StoryNode StrafeMissed => new StoryNode()
            {
                type = NodeTypeCombat,
                playerShotJustMissed = true,
                playerShotWasFromStrafe = true,
                oncePerCombat = true,
                doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
            };
            public static StoryNode WeAreMovingAroundALot => new StoryNode()
            {
                type = NodeTypeCombat,
                minMovesThisTurn = 3,
                oncePerRun = true,
            };
            public static StoryNode WeGotShard => new StoryNode()
            {
                type = NodeTypeCombat,
                lastTurnPlayerStatuses = NamesToStatuses(new() { "shard" }),
                oncePerCombat = true,
            };
        }

        public class Relevance0
        {
            public static StoryNode BatboyCowardice => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "batboy" },
                enemyIntent = "batboyIsACoward"
            };
            public static StoryNode ChunkThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "chunk" },
                maxTurnsThisCombat = 1
            };
            public static StoryNode CrabThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "crab" },
                maxTurnsThisCombat = 1,
                priority = true,
                oncePerCombatTags = new() { "CrabThreats" }
            };
            public static StoryNode EvilRiggsIsSeriousYouKnow => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "pirateBoss" },
                enemyIntent = "hardRiggsGetsMad",
                priority = true,
                oncePerCombatTags = new() { "RiggsBossGivesYouOneTurnToGetBackHere" }
            };
            public static StoryNode MinerGeneralShouts => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "miner" },
                enemyIntent = "wideBigAttack",
                oncePerCombat = true
            };
            public static StoryNode OldSpikeChattyPostRenameSpikeTwo => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "spike" },
                oncePerCombatTags = new() { "OldSpikeNewName" },
                maxTurnsThisCombat = 1,
                spikeName = "spiketwo"
            };
            public static StoryNode Pirate_1_1 => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = new() { "pirate" },
                requiredScenes = new() { "Pirate_1" },
                excludedScenes = new() { "Pirate_2" }
            };
            public static StoryNode SashaSportsShouts => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "sasha" },
                oncePerCombat = true,
                playerJustShotASoccerBall = true
            };
            public static StoryNode ScrapThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "scrap" },
                maxTurnsThisCombat = 1
            };
            public static StoryNode SogginsEscapeIntent_4 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                enemyIntent = "sogginsEscapeIntentPlease",
                turnStart = true,
                priority = true,
                oncePerRun = false,
                specialFight = "sogginsMissileEvent",
                lookup = new() { "sogginsPleaseLetMeLeave" },
                requiredScenes = new() { "SogginsEscapeIntent_3" }
            };
            public static StoryNode Soggins_Missile_Shout_2 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true,
                requiredScenes = new() { "Soggins_Missile_Shout_1" }
            };
            public static StoryNode Soggins_Missile_Shout_3 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true,
                requiredScenes = new() { "Soggins_Missile_Shout_2" }
            };
            public static StoryNode Soggins_Missile_Shout_4 => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "soggins" },
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true,
                requiredScenes = new() { "Soggins_Missile_Shout_3" }
            };
            public static StoryNode SpikeThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "oxygenguy" },
                maxTurnsThisCombat = 1,
                priority = true,
                oncePerCombatTags = new() { "SpikeThreats" }
            };
            public static StoryNode StardogSeesYouFlippedHisMissiles => new StoryNode()
            {
                type = NodeTypeCombat,
                allPresent = new() { "wolf" },
                oncePerCombatTags = new() { "StardogIsInTroubleNow" },
                priority = true,
                lookup = new() { "flippedMissile" }
            };
            public static StoryNode StardogThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "wolf" },
                maxTurnsThisCombat = 2,
                priority = true,
                oncePerCombatTags = new() { "StardogThreats" }
            };
            public static StoryNode TentacleThreatsOnShellBreak => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "tentacle" },
                priority = true,
                enemyIntent = "cracked",
                oncePerRunTags = new() { "youCrackedTheStarnacleShellBro" }
            };
            public static StoryNode TentacleThreats => new StoryNode()
            {
                type = NodeTypeCombat,
                turnStart = true,
                allPresent = new() { "tentacle" },
                maxTurnsThisCombat = 1,
                priority = true
            };
        }
    }


/*  FULL LIST OF ALL MULTIS IN THE CURRENT VERSION OF Story.json
    
ArtifactAresCannonV2
ArtifactAresCannon
ArtifactArmoredBay
ArtifactBerserkerDrive
ArtifactBrokenGlasses
ArtifactCockpitTargetIsNotRelevant
ArtifactCockpitTargetIsRelevant
ArtifactCrosslink
ArtifactDemonThrusters
ArtifactDirtyEngines
ArtifactDizzyBoost
ArtifactEnergyPrep
ArtifactEnergyRefund
ArtifactFlywheel
ArtifactFractureDetection
ArtifactGeminiCoreBooster
ArtifactGeminiCore
ArtifactGravelRecyclerGeode
ArtifactGravelRecycler
ArtifactHardmode
ArtifactHullPlatingWhenMissed
ArtifactHullPlatingWhenShot
ArtifactIonConverter
ArtifactJetThrustersNoRiggs
ArtifactJetThrusters
ArtifactJumperCablesUseless
ArtifactJumperCables
ArtifactNanofiberHull1
ArtifactNanofiberHull2
ArtifactNanofiberHullHealBooster
ArtifactOverclockedGeneratorSeenMaxMemory3
ArtifactOverclockedGenerator
ArtifactPiercerStrafe
ArtifactPiercer
ArtifactPowerDiversionMadeDizzyAttackFail
ArtifactPressureFuseFail
ArtifactPressureFuse
ArtifactQuickDraw
ArtifactRecalibrator
ArtifactRevengeDriveBig
ArtifactRevengeDrive
ArtifactSalvageArmDanger
ArtifactSalvageArm
ArtifactSharpEdges
ArtifactShieldPrepIsGone
ArtifactSimplicity
ArtifactTiderunner
ArtifactTridimensionalCockpit
ArtifactWarpMastery
BanditThreats
BatboyCowardice
BatboyKeepsTalking
BatboyThreats
Batboy_Infinite
BlockedALotOfAttacksWithArmor
BlockedAnEnemyAttackWithArmor
BooksJustHit
BooksWentMissing
CATJustHit
CATsummonedBooksCard
CATsummonedCATCard
CATsummonedDizzyCard
CATsummonedDrakeCard
CATsummonedIsaacCard
CATsummonedMaxCard
CATsummonedPeriCard
CATsummonedRiggsCard
CatWentMissing
CheapCardPlayed
ChunkThreats
CrabFacts1
CrabFacts2
CrabFactsAreOverNow
CrabThreats
DizzyBigHit
DizzyJustHit
DizzyWentMissing
DrakeWentMissing
Drone_battery_Destroyed
Drone_battery_Spawned
Drone_callisto_Destroyed
Drone_callisto_Spawned
Drone_cheebo_Destroyed
Drone_cheebo_Spawned
Drone_chosenone_Destroyed
Drone_chosenone_Spawned
Drone_europa_Destroyed
Drone_europa_Spawned
Drone_ganymede_Destroyed
Drone_ganymede_Spawned
Drone_gary_Destroyed
Drone_gary_Spawned
Drone_iggy_Destroyed
Drone_iggy_Spawned
Drone_io_Destroyed
Drone_io_Spawned
Drone_isaacjr_Destroyed
Drone_isaacjr_Spawned
Drone_itchy_Destroyed
Drone_itchy_Spawned
Drone_jeff_Destroyed
Drone_jeff_Spawned
Drone_juice_Destroyed
Drone_juice_Spawned
Drone_jupejr_Destroyed
Drone_jupejr_Spawned
Drone_king_Destroyed
Drone_king_Spawned
Drone_larry_Destroyed
Drone_larry_Spawned
Drone_lemmy_Destroyed
Drone_lemmy_Spawned
Drone_ludwig_Destroyed
Drone_ludwig_Spawned
Drone_lumpy_Destroyed
Drone_lumpy_Spawned
Drone_morton_Destroyed
Drone_morton_Spawned
Drone_namelessone_Destroyed
Drone_namelessone_Spawned
Drone_numberone_Destroyed
Drone_numberone_Spawned
Drone_roy_Destroyed
Drone_roy_Spawned
Drone_scoobert_Destroyed
Drone_scoobert_Spawned
Drone_sparky_Destroyed
Drone_sparky_Spawned
Drone_stinky_Destroyed
Drone_stinky_Spawned
Drone_wendy_Destroyed
Drone_wendy_Spawned
Duo_AboutToDieAndLoop
EmptyHandWithEnergy
EnemyArmorHitLots
EnemyArmorHit
EnemyArmorPierced
EnemyHasBrittle
EnemyHasWeakness
EuniceHitOncePerRun
EuniceJustHit
EuniceMiss
EvadeLastsBetweenTurns
EvilRiggsIsSeriousYouKnow
EvilRiggsWhenRiggsTriesToLeave
ExpensiveCardPlayed
GoatJustHit
HackerJustHit
HandOnlyHasTrashCards
HandOnlyHasUnplayableCards
IsaacHasTooMuchRockFactoryButYouHaveGravelRecycler
IsaacHasTooMuchRockFactoryButYouHaveSalvageArm
IsaacHasTooMuchRockFactory
IsaacWentMissing
JustHitGeneric
JustPlayedADraculaCard
JustPlayedASashaCard
JustPlayedASogginsCard
JustPlayedAToothCard
JustPlayedAnEphemeralCard
Knight_Midcombat_Greeting_Drake
Knight_Midcombat_Greeting_Infinite_Goat
Knight_Midcombat_Greeting_Infinite
LookOutMissile
ManyFlips
ManyTurns
MaxIsSuperIntoTheBandit
MaxWentMissing
MinerGeneralShouts
MinerOncePerFightShouts
Miner_Infinite
OldSpikeChattyPostRenameGeorge
OldSpikeChattyPostRenameSpikeTwo
OneHitPointThisIsFine
OopsBackwardsJupiter
OverheatCatFix
OverheatDrakeFix
OverheatDrakesFault
OverheatGeneric
PeriJustHitAndDidBigDamage
PeriJustHit
PeriWentMissing
PirateGeneralShouts
Pirate_Infinite_AfterCrew
Pirate_Infinite_BeforeCrew
PlayedManyCards
RiggsHandCannon
RiggsJustHit
RiggsWentMissing
SashaSportsShouts
Sasha_2
ScrapThreats
ShopNothing_1
ShopRemoveCard
ShopRemoveTwoCards
ShopUpgradeCard
ShopkeeperInfinite_Books
ShopkeeperInfinite_Comp
ShopkeeperInfinite_Dizzy
ShopkeeperInfinite_Eunice
ShopkeeperInfinite_Isaac
ShopkeeperInfinite_Max
ShopkeeperInfinite_Peri
ShopkeeperInfinite_Riggs
SpikeGetsChatty
SpikeThreats
StardogGetsChatty
StardogSeesYouFlippedHisMissiles
StardogThreats
StoneNervous
StoneReverse
StrafeHit
StrafeMissedGood
StrafeMissed
TentacleThreatsOnShellBreak
TentacleThreats
ThatsALotOfDamageToThem
ThatsALotOfDamageToUs
TheCobalt_Infinite
TheyGotCorroded
TheyHaveAutoDodgeLeft
TheyHaveAutoDodgeRight
TookDamageHave2HP
TookZeroDamageAtLowHealth
VeryManyTurns
VoidShout_Infinite
VoidShout_Soggins
WeAreCorroded
WeAreMovingAroundALot
WeDidOverFiveDamage
WeDidOverThreeDamage
WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith
WeDontOverlapWithEnemyAtAll
WeGotHurtButNotTooBad
WeGotShard
WeGotShotButTookNoDamage
WeJustGainedHeatAndDrakeIsHere
WeJustOverheated
WeMissedOopsie
WizardGeneralShouts



    */
}
