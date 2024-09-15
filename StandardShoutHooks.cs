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
        // TODO: replace this malarkey with Nickel's util methods
        private static Dictionary<string, Deck> NameToDeck_Cache = [];
        private static Deck NameToDeck(string name) 
        { 
            if (!NameToDeck_Cache.ContainsKey(name)) NameToDeck_Cache.Add(name, Enum.Parse<Deck>(name));
            return NameToDeck_Cache[name];
        }

        // All 8 crewmates comment on these events. Highly reccomended to write dialogue for these.
        public class Relevance8
        {
            // should not be referenced as a new shout, modify game's existing shout sayswitch for BanditThreats_Multi_0 specifically
            public static (string, StoryNode) BanditThreats => ("BanditThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "bandit" ],
                maxTurnsThisCombat = 1,
                priority = true,
                oncePerCombatTags = [ "BanditThreats" ]
            });
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static (string, StoryNode) CrabFacts1 => ("CrabFacts1", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "crab" ],
                priority = true,
                minTurnsThisCombat = 2,
                maxTurnsThisCombat = 2
            });
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static (string, StoryNode) CrabFacts2 => ("CrabFacts2", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "crab" ],
                priority = true,
                minTurnsThisCombat = 3,
                maxTurnsThisCombat = 3
            });
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static (string, StoryNode) CrabFactsAreOverNow => ("CrabFactsAreOverNow", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "crab" ],
                priority = true,
                minTurnsThisCombat = 4,
                maxTurnsThisCombat = 4
            });
            public static (string, StoryNode) Duo_AboutToDieAndLoop => ("Duo_AboutToDieAndLoop", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                oncePerCombatTags = [ "aboutToDie" ],
                oncePerRun = true,
            });
            public static (string, StoryNode) EmptyHandWithEnergy => ("EmptyHandWithEnergy", new StoryNode()
            {
                type = NodeType.combat,
                handEmpty = true,
                minEnergy = 1,
               
            });
            public static (string, StoryNode) JustHitGeneric => ("JustHitGeneric", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
            });
            public static (string, StoryNode) OverheatDrakeFix => ("OverheatDrakeFix", new StoryNode()
            {
                type = NodeType.combat,
                wasGoingToOverheatButStopped = true,
                whoDidThat = Deck.eunice,
                allPresent = [ "eunice" ],
                oncePerCombatTags = [ "OverheatDrakeFix" ]
            });
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static (string, StoryNode) ShopKeepBattleInsult => ("ShopKeepBattleInsult", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                allPresent = [ "nerd" ],
                oncePerRunTags = [ "ShopkeepAboutToDestroyYou" ],
                enemyIntent = "shopkeepAttack"
            });
            public static (string, StoryNode) VeryManyTurns => ("VeryManyTurns", new StoryNode()
            {
                type = NodeType.combat,
                minTurnsThisCombat = 20,
                oncePerCombatTags = [ "veryManyTurns" ],
                oncePerRun = true,
                turnStart = true,
               
            });
        }

        public class Relevance7
        {
            public static (string, StoryNode) ArtifactGeminiCore => ("ArtifactGeminiCore", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "GeminiCore" ],
                oncePerRunTags = [ "GeminiCore" ],
               
            });
            public static (string, StoryNode) ArtifactTiderunner => ("ArtifactTiderunner", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "TideRunner" ],
                oncePerCombatTags = [ "TideRunner" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactTridimensionalCockpit => ("ArtifactTridimensionalCockpit", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerCombatTags = [ "TridimensionalCockpit" ],
                hasArtifacts = [ "TridimensionalCockpit" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) EnemyHasBrittle => ("EnemyHasBrittle", new StoryNode()
            {
                type = NodeType.combat,
                enemyHasBrittlePart = true,
                oncePerRunTags = [ "yelledAboutBrittle" ],
               
            });
            public static (string, StoryNode) EnemyHasWeakness => ("EnemyHasWeakness", new StoryNode()
            {
                type = NodeType.combat,
                enemyHasWeakPart = true,
                oncePerRunTags = [ "yelledAboutWeakness" ],
               
            });
            public static (string, StoryNode) JustPlayedADraculaCard => ("JustPlayedADraculaCard", new StoryNode()
            {
                type = NodeType.combat,
                whoDidThat = Deck.dracula,
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ManyTurns => ("ManyTurns", new StoryNode()
            {
                type = NodeType.combat,
                minTurnsThisCombat = 9,
                oncePerCombatTags = [ "manyTurns" ],
                oncePerRun = true,
                turnStart = true,
               
            });
            public static (string, StoryNode) OneHitPointThisIsFine => ("OneHitPointThisIsFine", new StoryNode()
            {
                type = NodeType.combat,
                oncePerCombatTags = [ "aboutToDie" ],
                oncePerRun = true,
                enemyShotJustHit = true,
                maxHull = 1,
               
            });
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static (string, StoryNode) SogginsEscapeIntent_1 => ("SogginsEscapeIntent_1", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                enemyIntent = "sogginsEscapeIntent",
                turnStart = true,
                priority = true,
                oncePerRun = true,
                specialFight = "sogginsMissileEvent"
            });
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static (string, StoryNode) Soggins_Missile_Shout_1 => ("Soggins_Missile_Shout_1", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true
            });
            public static (string, StoryNode) ThatsALotOfDamageToUs => ("ThatsALotOfDamageToUs", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageDealtToPlayerThisTurn = 3,
               
            });
            public static (string, StoryNode) WeDidOverFiveDamage => ("WeDidOverFiveDamage", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 6,
               
            });
            // should not be referenced as a new shout, modify game's existing shout sayswitch
            public static (string, StoryNode) WeJustGainedHeatAndDrakeIsHere => ("WeJustGainedHeatAndDrakeIsHere", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.heat ],
                allPresent = [ "eunice" ],
                oncePerCombatTags = [ "DrakeCanYouDoSomethingAboutTheHeatPlease" ]
            });
        }

        public class Relevance6
        {
            public static (string, StoryNode) ArtifactRecalibrator => ("ArtifactRecalibrator", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustMissed = true,
                hasArtifacts = [ "Recalibrator" ],
               
            });
            public static (string, StoryNode) BooksWentMissing => ("BooksWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingBooks ],
                priority = true,
                oncePerCombatTags = [ "booksWentMissing" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) CatWentMissing => ("CatWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingCat ],
                priority = true,
                oncePerCombatTags = [ "CatWentMissing" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) HandOnlyHasTrashCards => ("HandOnlyHasTrashCards", new StoryNode()
            {
                type = NodeType.combat,
                handFullOfTrash = true,
                oncePerCombatTags = [ "handOnlyHasTrashCards" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) OverheatGeneric => ("OverheatGeneric", new StoryNode()
            {
                type = NodeType.combat,
                goingToOverheat = true,
                oncePerCombatTags = [ "OverheatGeneric" ],
               
            });
            public static (string, StoryNode) RiggsWentMissing => ("RiggsWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingRiggs ],
                priority = true,
                oncePerCombatTags = [ "riggsWentMissing" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ThatsALotOfDamageToThem => ("ThatsALotOfDamageToThem", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 10,
               
            });
            public static (string, StoryNode) TheyHaveAutoDodgeLeft => ("TheyHaveAutoDodgeLeft", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnEnemyStatuses = [ Status.autododgeLeft ],
                oncePerCombatTags = [ "aboutAutododge" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) TheyHaveAutoDodgeRight => ("TheyHaveAutoDodgeRight", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnEnemyStatuses = [ Status.autododgeRight ],
                oncePerCombatTags = [ "aboutAutododge" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) WeAreCorroded => ("WeAreCorroded", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.corrode ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) WeDidOverThreeDamage => ("WeDidOverThreeDamage", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 4,
               
            });
            public static (string, StoryNode) WeDontOverlapWithEnemyAtAll => ("WeDontOverlapWithEnemyAtAll", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                shipsDontOverlapAtAll = true,
                oncePerCombatTags = [ "NoOverlapBetweenShips" ],
                oncePerRun = true,
                nonePresent = [ "crab", "scrap" ],
               
            });
            public static (string, StoryNode) WeGotHurtButNotTooBad => ("WeGotHurtButNotTooBad", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageDealtToPlayerThisTurn = 1,
                maxDamageDealtToPlayerThisTurn = 1,
               
            });
            public static (string, StoryNode) WeGotShotButTookNoDamage => ("WeGotShotButTookNoDamage", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxDamageDealtToPlayerThisTurn = 0,
            });
            public static (string, StoryNode) WeMissedOopsie => ("WeMissedOopsie", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustMissed = true,
                oncePerCombat = true,
                doesNotHaveArtifacts = [ "Recalibrator", "GrazerBeam" ],
               
            });
        }

        public class Relevance5
        {
            public static (string, StoryNode) ArtifactCockpitTargetIsRelevant => ("ArtifactCockpitTargetIsRelevant", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                enemyHasPart = "cockpit",
                hasArtifacts = [ "CockpitTarget" ],
               
            });
            public static (string, StoryNode) ArtifactCrosslink => ("ArtifactCrosslink", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "Crosslink" ],
                lookup = [ "CrosslinkTrigger" ],
                oncePerRun = true,
                oncePerCombatTags = [ "CrosslinkTriggerTag" ],
               
            });
            public static (string, StoryNode) ArtifactFractureDetection => ("ArtifactFractureDetection", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "FractureDetection" ],
                oncePerCombatTags = [ "FractureDetectionBarks" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactJetThrusters => ("ArtifactJetThrusters", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "JetThrusters" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactNanofiberHull1 => ("ArtifactNanofiberHull1", new StoryNode()
            {
                type = NodeType.combat,
                minDamageDealtToPlayerThisTurn = 1,
                maxDamageDealtToPlayerThisTurn = 1,
                hasArtifacts = [ "NanofiberHull" ],
                oncePerRunTags = [ "NanofiberHull" ],
               
            });
            public static (string, StoryNode) BlockedAnEnemyAttackWithArmor => ("BlockedAnEnemyAttackWithArmor", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 1,
                oncePerCombatTags = [ "WowArmorISPrettyCoolHuh" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) DizzyWentMissing => ("DizzyWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingDizzy ],
                priority = true,
                oncePerCombatTags = [ "dizzyWentMissing" ],
               
            });
            public static (string, StoryNode) DrakeWentMissing => ("DrakeWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingDrake ],
                priority = true,
                oncePerCombatTags = [ "drakeWentMissing" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) EnemyArmorHit => ("EnemyArmorHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageBlockedByEnemyArmorThisTurn = 1,
                oncePerCombat = true,
                oncePerRun = true,
               
            });
            public static (string, StoryNode) IsaacWentMissing => ("IsaacWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingIsaac ],
                priority = true,
                oncePerCombatTags = [ "isaacWentMissing" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) LookOutMissile => ("LookOutMissile", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                once = true,
                oncePerRunTags = [ "goodMissileAdvice" ],
                anyDronesHostile = [ "missile_normal", "missile_heavy", "missile_corrode", "missile_seeker", "missile_breacher" ],
               
            });
            public static (string, StoryNode) MaxWentMissing => ("MaxWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingMax ],
                priority = true,
                oncePerCombatTags = [ "maxWentMissing" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) OverheatDrakesFault => ("OverheatDrakesFault", new StoryNode()
            {
                type = NodeType.combat,
                goingToOverheat = true,
                whoDidThat = Deck.eunice,
                allPresent = [ "eunice" ],
                oncePerCombatTags = [ "OverheatDrakesFault" ]
            });
            public static (string, StoryNode) PeriWentMissing => ("PeriWentMissing", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.missingPeri ],
                priority = true,
                oncePerCombatTags = [ "periWentMissing" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) SpikeGetsChatty => ("SpikeGetsChatty", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "oxygenguy" ],
                oncePerCombatTags = [ "SpikeGoAway" ]
            });
            public static (string, StoryNode) StrafeHit => ("StrafeHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                playerShotWasFromStrafe = true,
                oncePerCombat = true,
               
            });
            public static (string, StoryNode) TheyGotCorroded => ("TheyGotCorroded", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnEnemyStatuses = [ Status.corrode ],
                oncePerRun = true,
               
            });
        }

        public class Relevance4
        {
            public static (string, StoryNode) ArtifactAresCannon => ("ArtifactAresCannon", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "AresCannon" ],
                oncePerRunTags = [ "AresCannon" ],
               
            });
            public static (string, StoryNode) ArtifactBrokenGlasses => ("ArtifactBrokenGlasses", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                hasArtifacts = [ "BrokenGlasses" ],
               
            });
            public static (string, StoryNode) ArtifactEnergyRefund => ("ArtifactEnergyRefund", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "EnergyRefund" ],
                minCostOfCardJustPlayed = 3,
                oncePerCombatTags = [ "EnergyRefund" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactNanofiberHull2 => ("ArtifactNanofiberHull2", new StoryNode()
            {
                type = NodeType.combat,
                minDamageDealtToPlayerThisTurn = 2,
                hasArtifacts = [ "NanofiberHull" ],
                oncePerRunTags = [ "NanofiberHull2" ],
               
            });
            public static (string, StoryNode) ArtifactOverclockedGeneratorSeenMaxMemory3 => ("ArtifactOverclockedGeneratorSeenMaxMemory3", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "OverclockedGenerator" ],
                priority = true,
                oncePerRunTags = [ "OverclockedGeneratorTag" ],
                lookup = [ "OverclockedGeneratorTrigger" ],
                requiredScenes = [ "Hacker_Memory_3" ],
               
            });
            public static (string, StoryNode) ArtifactShieldPrepIsGone => ("ArtifactShieldPrepIsGone", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = [ "ShieldPrepIsGoneYouFool" ],
                doesNotHaveArtifacts = [ "ShieldPrep", "WarpMastery" ],
               
            });
            public static (string, StoryNode) ArtifactWarpMastery => ("ArtifactWarpMastery", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRunTags = [ "WarpMastery" ],
                hasArtifacts = [ "WarpMastery" ],
               
            });
            public static (string, StoryNode) BatboyKeepsTalking => ("BatboyKeepsTalking", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "batboy" ],
                oncePerCombatTags = [ "batboyshutupyouBigNERD" ]
            });
            public static (string, StoryNode) BlockedALotOfAttacksWithArmor => ("BlockedALotOfAttacksWithArmor", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 3,
                oncePerCombatTags = [ "YowzaThatWasALOTofArmorBlock" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) JustPlayedASashaCard => ("JustPlayedASashaCard", new StoryNode()
            {
                type = NodeType.combat,
                whoDidThat = Deck.sasha,
                oncePerRunTags = [ "usedASashaCard" ]
            });
            public static (string, StoryNode) SogginsEscapeIntent_3 => ("SogginsEscapeIntent_3", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                enemyIntent = "sogginsEscapeIntentPlease",
                turnStart = true,
                priority = true,
                oncePerRun = true,
                specialFight = "sogginsMissileEvent",
                requiredScenes = [ "SogginsEscapeIntent_2" ]
            });
            public static (string, StoryNode) WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith => ("WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                shipsDontOverlapAtAll = true,
                oncePerCombatTags = [ "NoOverlapBetweenShipsSeeker" ],
                anyDronesHostile = [ "missile_seeker" ],
                nonePresent = [ "crab" ],
               
            });
        }

        public class Relevance3
        {
            public static (string, StoryNode) ArtifactArmoredBay => ("ArtifactArmoredBay", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                minDamageBlockedByPlayerArmorThisTurn = 1,
                oncePerCombatTags = [ "ArmoredBae" ],
                hasArtifacts = [ "ArmoredBay" ],
               
            });
            public static (string, StoryNode) ArtifactCockpitTargetIsNotRelevant => ("ArtifactCockpitTargetIsNotRelevant", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                oncePerRun = true,
                enemyDoesNotHavePart = "cockpit",
                hasArtifacts = [ "CockpitTarget" ],
               
            });
            public static (string, StoryNode) ArtifactDizzyBoost => ("ArtifactDizzyBoost", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                whoDidThat = Deck.dizzy,
                hasArtifacts = [ "DizzyBoost" ],
                oncePerCombat = true,
                allPresent = [ "dizzy" ]
            });
            public static (string, StoryNode) ArtifactEnergyPrep => ("ArtifactEnergyPrep", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "EnergyPrep" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactGeminiCoreBooster => ("ArtifactGeminiCoreBooster", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "GeminiCoreBooster" ],
                oncePerRunTags = [ "GeminiCoreBooster" ],
               
            });
            public static (string, StoryNode) ArtifactHardmode => ("ArtifactHardmode", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                hasArtifacts = [ "HARDMODE" ],
                oncePerRunTags = [ "HARDMODE" ],
                once = true,
               
            });
            public static (string, StoryNode) ArtifactHullPlatingWhenShot => ("ArtifactHullPlatingWhenShot", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                oncePerRun = true,
                hasArtifacts = [ "HullPlating" ],
               
            });
            public static (string, StoryNode) ArtifactIonConverter => ("ArtifactIonConverter", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "IonConverter" ],
                lookup = [ "IonConverterTrigger" ],
                oncePerRun = true,
                priority = true,
                oncePerCombatTags = [ "IonConverterTag" ],
               
            });
            public static (string, StoryNode) ArtifactOverclockedGenerator => ("ArtifactOverclockedGenerator", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "OverclockedGenerator" ],
                oncePerRunTags = [ "OverclockedGeneratorTag" ],
                lookup = [ "OverclockedGeneratorTrigger" ],
               
            });
            public static (string, StoryNode) ArtifactPiercer => ("ArtifactPiercer", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "Piercer" ],
                oncePerCombatTags = [ "PiercerShouts" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactPowerDiversionMadeDizzyAttackFail => ("ArtifactPowerDiversionMadeDizzyAttackFail", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                maxDamageDealtToEnemyThisAction = 0,
                whoDidThat = Deck.dizzy,
                hasArtifacts = [ "PowerDiversion" ],
                allPresent = [ "dizzy", "peri" ]
            });
            public static (string, StoryNode) ArtifactQuickDraw => ("ArtifactQuickDraw", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "Quickdraw" ],
               
            });
            public static (string, StoryNode) ArtifactSimplicity => ("ArtifactSimplicity", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRunTags = [ "SimplicityShouts" ],
                hasArtifacts = [ "Simplicity" ],
               
            });
            public static (string, StoryNode) BooksJustHit => ("BooksJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = Deck.shard,
               
                oncePerCombatTags = [ "BooksShotThatGuy" ]
            });
            public static (string, StoryNode) CATJustHit => ("CATJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = Deck.colorless,
               
            });
            public static (string, StoryNode) CheapCardPlayed => ("CheapCardPlayed", new StoryNode()
            {
                type = NodeType.combat,
                maxCostOfCardJustPlayed = 0,
                oncePerCombatTags = [ "CheapCardPlayed" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) DizzyJustHit => ("DizzyJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = Deck.dizzy,
               
            });
            public static (string, StoryNode) EvadeLastsBetweenTurns => ("EvadeLastsBetweenTurns", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                once = true,
                oncePerCombatTags = [ "goodEvadeAdvice" ],
                lastTurnPlayerStatuses = [ Status.evade ],
                minRuns = 1,
               
            });
            public static (string, StoryNode) ExpensiveCardPlayed => ("ExpensiveCardPlayed", new StoryNode()
            {
                type = NodeType.combat,
                minCostOfCardJustPlayed = 4,
                oncePerCombatTags = [ "ExpensiveCardPlayed" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) HackerJustHit => ("HackerJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = Deck.hacker,
               
            });
            public static (string, StoryNode) HandOnlyHasUnplayableCards => ("HandOnlyHasUnplayableCards", new StoryNode()
            {
                type = NodeType.combat,
                handFullOfUnplayableCards = true,
                oncePerCombatTags = [ "handFullOfUnplayableCards" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) JustPlayedASogginsCard => ("JustPlayedASogginsCard", new StoryNode()
            {
                type = NodeType.combat,
                whoDidThat = Deck.soggins,
                oncePerRun = true,
               
            });
            public static (string, StoryNode) PeriJustHit => ("PeriJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                oncePerRun = true,
                oncePerCombatTags = [ "PeriHitEmYo" ],
                whoDidThat = Deck.peri,
               
            });
            public static (string, StoryNode) PlayedManyCards => ("PlayedManyCards", new StoryNode()
            {
                type = NodeType.combat,
                handEmpty = true,
                minCardsPlayedThisTurn = 6,
               
            });
            public static (string, StoryNode) StardogGetsChatty => ("StardogGetsChatty", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "wolf" ],
                oncePerCombatTags = [ "StardogLeaveUsAlone" ]
            });
            public static (string, StoryNode) TookDamageHave2HP => ("TookDamageHave2HP", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                minHull = 2,
                maxHull = 2,
                oncePerRunTags = [ "TookDamageHave2HP" ],
               
            });
            public static (string, StoryNode) TookZeroDamageAtLowHealth => ("TookZeroDamageAtLowHealth", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxDamageDealtToPlayerThisTurn = 0,
                maxHull = 2,
               
            });
            public static (string, StoryNode) WizardGeneralShouts => ("WizardGeneralShouts", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "wizard" ],
                enemyIntent = "wizardMagic"
            });
        }

        public class Relevance2
        {
            public static (string, StoryNode) ArtifactAresCannonV2 => ("ArtifactAresCannonV2", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "AresCannonV2" ],
                oncePerRunTags = [ "AresCannonV2" ],
               
            });
            public static (string, StoryNode) ArtifactDirtyEngines => ("ArtifactDirtyEngines", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                hasArtifacts = [ "DirtyEngines" ],
               
            });
            public static (string, StoryNode) ArtifactFlywheel => ("ArtifactFlywheel", new StoryNode()
            {
                type = NodeType.combat,
                hasArtifacts = [ "Flywheel" ],
                oncePerRunTags = [ "Flywheel" ],
               
            });
            public static (string, StoryNode) ArtifactGravelRecyclerGeode => ("ArtifactGravelRecyclerGeode", new StoryNode()
            {
                type = NodeType.combat,
                anyDrones = [ "asteroidShard" ],
                hasArtifacts = [ "GravelRecycler" ],
                oncePerCombatTags = [ "GravelRecyclerGeode" ],
               
            });
            public static (string, StoryNode) ArtifactGravelRecycler => ("ArtifactGravelRecycler", new StoryNode()
            {
                type = NodeType.combat,
                anyDrones = [ "asteroid" ],
                hasArtifacts = [ "GravelRecycler" ],
                oncePerCombatTags = [ "GravelRecycler" ],
               
            });
            public static (string, StoryNode) ArtifactJetThrustersNoRiggs => ("ArtifactJetThrustersNoRiggs", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                maxTurnsThisCombat = 1,
                hasArtifacts = [ "JetThrusters" ],
                nonePresent = [ "riggs" ],
                oncePerRunTags = [ "OncePerRunThrusterJokesAboutRiggsICanMakeTheseTagsStupidlyLongIfIWant" ],
               
            });
            public static (string, StoryNode) ArtifactNanofiberHullHealBooster => ("ArtifactNanofiberHullHealBooster", new StoryNode()
            {
                type = NodeType.combat,
                minDamageDealtToPlayerThisTurn = 2,
                maxDamageDealtToPlayerThisTurn = 2,
                hasArtifacts = [ "NanofiberHull", "HealBooster" ],
                oncePerRunTags = [ "NanofiberHull3" ],
               
            });
            public static (string, StoryNode) CATsummonedIsaacCard => ("CATsummonedIsaacCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonIsaac" ],
                oncePerCombatTags = [ "summonIsaacTag" ],
                oncePerRun = true,
            });
            public static (string, StoryNode) CATsummonedPeriCard => ("CATsummonedPeriCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonPeri" ],
                oncePerCombatTags = [ "summonPeriTag" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) CATsummonedRiggsCard => ("CATsummonedRiggsCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonRiggs" ],
                oncePerCombatTags = [ "summonRiggsTag" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) Crystal_1_2 => ("Crystal_1_2", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "crystal" ],
                requiredScenes = [ "Crystal_1", "Crystal_1_1" ],
                excludedScenes = [ "Crystal_2" ]
            });
            public static (string, StoryNode) EnemyArmorHitLots => ("EnemyArmorHitLots", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageBlockedByEnemyArmorThisTurn = 3,
                oncePerCombat = true,
                oncePerRun = true,
               
            });
            public static (string, StoryNode) EnemyArmorPierced => ("EnemyArmorPierced", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                playerJustPiercedEnemyArmor = true,
                oncePerCombatTags = [ "EnemyArmorPierced" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) EuniceJustHit => ("EuniceJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = Deck.eunice,
               
            });
            public static (string, StoryNode) GoatJustHit => ("GoatJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = Deck.goat,
               
            });
            public static (string, StoryNode) JustPlayedAToothCard => ("JustPlayedAToothCard", new StoryNode()
            {
                type = NodeType.combat,
                whoDidThat = Deck.tooth,
                oncePerRunTags = [ "usedAToothCard" ],
               
            });
            public static (string, StoryNode) ManyFlips => ("ManyFlips", new StoryNode()
            {
                type = NodeType.combat,
                minTimesYouFlippedACardThisTurn = 4,
                oncePerCombat = true,
               
            });
            public static (string, StoryNode) OverheatCatFix => ("OverheatCatFix", new StoryNode()
            {
                type = NodeType.combat,
                wasGoingToOverheatButStopped = true,
                whoDidThat = Deck.colorless,
               
                oncePerCombatTags = [ "OverheatCatFix" ]
            });
            public static (string, StoryNode) PeriJustHitAndDidBigDamage => ("PeriJustHitAndDidBigDamage", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 3,
                whoDidThat = Deck.peri,
               
            });
            public static (string, StoryNode) RiggsJustHit => ("RiggsJustHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 1,
                whoDidThat = Deck.riggs,
               
            });
            public static (string, StoryNode) RiggsSeesDrakesCoolCard => ("RiggsSeesDrakesCoolCard", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                lookup = [ "drakesCoolCard" ],
                once = true,
                allPresent = [ "eunice", "riggs" ]
            });
            public static (string, StoryNode) StrafeMissedGood => ("StrafeMissedGood", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustMissed = true,
                playerShotWasFromStrafe = true,
                hasArtifacts = [ "Recalibrator", "GrazerBeam" ],
                oncePerCombat = true,
               
            });
            public static (string, StoryNode) TheCobalt_1_1 => ("TheCobalt_1_1", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "theCobalt" ],
                requiredScenes = [ "TheCobalt_1" ],
                excludedScenes = [ "TheCobalt_2" ]
            });
            public static (string, StoryNode) TheCobalt_1_2 => ("TheCobalt_1_2", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "theCobalt" ],
                requiredScenes = [ "TheCobalt_1", "TheCobalt_1_1" ],
                excludedScenes = [ "TheCobalt_2" ]
            });
            public static (string, StoryNode) TheCobalt_1_3 => ("TheCobalt_1_3", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "theCobalt" ],
                requiredScenes = [ "TheCobalt_1", "TheCobalt_1_2" ],
                excludedScenes = [ "TheCobalt_2" ]
            });
            public static (string, StoryNode) WeJustOverheated => ("WeJustOverheated", new StoryNode()
            {
                type = NodeType.combat,
                justOverheated = true,
                oncePerCombatTags = [ "WeJustOverheated" ],
            });
        }

        public class Relevance1
        {
            public static (string, StoryNode) ArtifactBerserkerDrive => ("ArtifactBerserkerDrive", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisTurn = 8,
                oncePerRun = true,
                hasArtifacts = [ "BerserkerDrive" ],
               
            });
            public static (string, StoryNode) ArtifactDemonThrusters => ("ArtifactDemonThrusters", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                hasArtifacts = [ "DemonThrusters" ],
                oncePerRunTags = [ "ArtifactDemonThrusters" ],
               
            });
            public static (string, StoryNode) ArtifactHullPlatingWhenMissed => ("ArtifactHullPlatingWhenMissed", new StoryNode()
            {
                type = NodeType.combat,
                enemyShotJustHit = true,
                oncePerRun = true,
                hasArtifacts = [ "HullPlating" ],
               
            });
            public static (string, StoryNode) ArtifactJumperCablesUseless => ("ArtifactJumperCablesUseless", new StoryNode()
            {
                type = NodeType.combat,
                maxTurnsThisCombat = 1,
                minHullPercent = 1,
                hasArtifacts = [ "JumperCables" ],
                oncePerRunTags = [ "ArtifactJumperCablesUnneeded" ],
               
            });
            public static (string, StoryNode) ArtifactJumperCables => ("ArtifactJumperCables", new StoryNode()
            {
                type = NodeType.combat,
                maxTurnsThisCombat = 1,
                maxHullPercent = 0.5,
                hasArtifacts = [ "JumperCables" ],
                oncePerRunTags = [ "ArtifactJumperCablesReady" ],
               
            });
            public static (string, StoryNode) ArtifactPiercerStrafe => ("ArtifactPiercerStrafe", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                playerShotWasFromStrafe = true,
                playerShotJustHit = true,
                hasArtifacts = [ "Piercer" ],
                oncePerCombatTags = [ "PiercerShoutsStrafe" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactPressureFuseFail => ("ArtifactPressureFuseFail", new StoryNode()
            {
                type = NodeType.combat,
                minHullPercent = 0.51,
                oncePerCombatTags = [ "PressureFuseFail" ],
                hasArtifacts = [ "PressureFuse" ],
               
            });
            public static (string, StoryNode) ArtifactPressureFuse => ("ArtifactPressureFuse", new StoryNode()
            {
                type = NodeType.combat,
                maxHullPercent = 0.5,
                oncePerCombatTags = [ "PressureFuse" ],
                hasArtifacts = [ "PressureFuse" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) ArtifactRevengeDriveBig => ("ArtifactRevengeDriveBig", new StoryNode()
            {
                type = NodeType.combat,
                minDamageDealtToPlayerThisTurn = 3,
                oncePerCombat = true,
                enemyShotJustHit = true,
                hasArtifacts = [ "RevengeDrive" ],
               
            });
            public static (string, StoryNode) ArtifactRevengeDrive => ("ArtifactRevengeDrive", new StoryNode()
            {
                type = NodeType.combat,
                minDamageDealtToPlayerThisTurn = 1,
                enemyShotJustHit = true,
                hasArtifacts = [ "RevengeDrive" ],
                oncePerCombatTags = [ "RevengeDriveShouts" ],
               
            });
            public static (string, StoryNode) ArtifactSalvageArmDanger => ("ArtifactSalvageArmDanger", new StoryNode()
            {
                type = NodeType.combat,
                anyDronesHostile = [ "missile_normal", "missile_heavy", "missile_corrode", "missile_seeker", "missile_breacher" ],
                oncePerCombatTags = [ "SalvageMissileAdvice" ],
                hasArtifacts = [ "SalvageArm" ],
               
            });
            public static (string, StoryNode) ArtifactSalvageArm => ("ArtifactSalvageArm", new StoryNode()
            {
                type = NodeType.combat,
                anyDrones = [ "asteroid" ],
                hasArtifacts = [ "SalvageArm" ],
                oncePerCombatTags = [ "salvageAsteroidAdvice" ],
               
            });
            public static (string, StoryNode) ArtifactSharpEdges => ("ArtifactSharpEdges", new StoryNode()
            {
                type = NodeType.combat,
                oncePerCombat = true,
                playerJustShuffledDiscardIntoDrawPile = true,
                hasArtifacts = [ "SharpEdges" ],
                oncePerRun = true,
               
            });
            public static (string, StoryNode) BatboyThreats => ("BatboyThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
               
                maxTurnsThisCombat = 1
            });
            public static (string, StoryNode) CATsummonedBooksCard => ("CATsummonedBooksCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonBooks" ],
                oncePerCombatTags = [ "summonBooksTag" ],
                oncePerRun = true,
            });
            public static (string, StoryNode) CATsummonedCATCard => ("CATsummonedCATCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonCAT" ],
                oncePerCombatTags = [ "summonCATTag" ],
                oncePerRun = true,
            });
            public static (string, StoryNode) CATsummonedDizzyCard => ("CATsummonedDizzyCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonDizzy" ],
                oncePerCombatTags = [ "summonDizzyTag" ],
                oncePerRun = true,
            });
            public static (string, StoryNode) CATsummonedDrakeCard => ("CATsummonedDrakeCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonDrake" ],
                oncePerCombatTags = [ "summonDrakeTag" ],
                oncePerRun = true,
            });
            public static (string, StoryNode) CATsummonedMaxCard => ("CATsummonedMaxCard", new StoryNode()
            {
                type = NodeType.combat,
                lookup = [ "summonMax" ],
                oncePerCombatTags = [ "summonMaxTag" ],
                oncePerRun = true,
            });
            public static (string, StoryNode) Crystal_1_1 => ("Crystal_1_1", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "crystal" ],
                requiredScenes = [ "Crystal_1" ],
                excludedScenes = [ "Crystal_2" ]
            });
            public static (string, StoryNode) Crystal_1_3 => ("Crystal_1_3", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "crystal" ],
                requiredScenes = [ "Crystal_1", "Crystal_1_2" ],
                excludedScenes = [ "Crystal_2" ]
            });
            public static (string, StoryNode) DizzyBigHit => ("DizzyBigHit", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 3,
                oncePerRunTags = [ "DizzyBigHit" ],
                whoDidThat = Deck.dizzy,
                allPresent = [ "dizzy" ]
            });
            public static (string, StoryNode) DrakeBot_1_1 => ("DrakeBot_1_1", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "drakebot", "eunice" ],
                requiredScenes = [ "DrakeBot_1" ],
                excludedScenes = [ "DrakeBot_2" ]
            });
            public static (string, StoryNode) Drone_battery_Destroyed => ("Drone_battery_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "battery"
            });
            public static (string, StoryNode) Drone_battery_Spawned => ("Drone_battery_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "battery"
            });
            public static (string, StoryNode) Drone_callisto_Destroyed => ("Drone_callisto_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "callisto"
            });
            public static (string, StoryNode) Drone_callisto_Spawned => ("Drone_callisto_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "callisto"
            });
            public static (string, StoryNode) Drone_cheebo_Destroyed => ("Drone_cheebo_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "cheebo"
            });
            public static (string, StoryNode) Drone_cheebo_Spawned => ("Drone_cheebo_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "cheebo"
            });
            public static (string, StoryNode) Drone_chosenone_Destroyed => ("Drone_chosenone_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "chosenone"
            });
            public static (string, StoryNode) Drone_chosenone_Spawned => ("Drone_chosenone_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "chosenone"
            });
            public static (string, StoryNode) Drone_europa_Destroyed => ("Drone_europa_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "europa"
            });
            public static (string, StoryNode) Drone_europa_Spawned => ("Drone_europa_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "europa"
            });
            public static (string, StoryNode) Drone_ganymede_Destroyed => ("Drone_ganymede_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "ganymede"
            });
            public static (string, StoryNode) Drone_ganymede_Spawned => ("Drone_ganymede_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "ganymede"
            });
            public static (string, StoryNode) Drone_gary_Destroyed => ("Drone_gary_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "gary"
            });
            public static (string, StoryNode) Drone_gary_Spawned => ("Drone_gary_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "gary"
            });
            public static (string, StoryNode) Drone_iggy_Destroyed => ("Drone_iggy_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "iggy"
            });
            public static (string, StoryNode) Drone_iggy_Spawned => ("Drone_iggy_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "iggy"
            });
            public static (string, StoryNode) Drone_io_Destroyed => ("Drone_io_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "io"
            });
            public static (string, StoryNode) Drone_io_Spawned => ("Drone_io_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "io"
            });
            public static (string, StoryNode) Drone_isaacjr_Destroyed => ("Drone_isaacjr_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "isaacjr"
            });
            public static (string, StoryNode) Drone_isaacjr_Spawned => ("Drone_isaacjr_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "isaacjr"
            });
            public static (string, StoryNode) Drone_itchy_Destroyed => ("Drone_itchy_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "itchy"
            });
            public static (string, StoryNode) Drone_itchy_Spawned => ("Drone_itchy_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "itchy"
            });
            public static (string, StoryNode) Drone_jeff_Destroyed => ("Drone_jeff_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "jeff"
            });
            public static (string, StoryNode) Drone_jeff_Spawned => ("Drone_jeff_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "jeff"
            });
            public static (string, StoryNode) Drone_juice_Destroyed => ("Drone_juice_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "juice"
            });
            public static (string, StoryNode) Drone_juice_Spawned => ("Drone_juice_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "juice"
            });
            public static (string, StoryNode) Drone_jupejr_Destroyed => ("Drone_jupejr_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "jupejr"
            });
            public static (string, StoryNode) Drone_jupejr_Spawned => ("Drone_jupejr_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "jupejr"
            });
            public static (string, StoryNode) Drone_king_Destroyed => ("Drone_king_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "king"
            });
            public static (string, StoryNode) Drone_king_Spawned => ("Drone_king_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "king"
            });
            public static (string, StoryNode) Drone_larry_Destroyed => ("Drone_larry_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "larry"
            });
            public static (string, StoryNode) Drone_larry_Spawned => ("Drone_larry_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "larry"
            });
            public static (string, StoryNode) Drone_lemmy_Destroyed => ("Drone_lemmy_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "lemmy"
            });
            public static (string, StoryNode) Drone_lemmy_Spawned => ("Drone_lemmy_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "lemmy"
            });
            public static (string, StoryNode) Drone_ludwig_Destroyed => ("Drone_ludwig_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "ludwig"
            });
            public static (string, StoryNode) Drone_ludwig_Spawned => ("Drone_ludwig_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "ludwig"
            });
            public static (string, StoryNode) Drone_lumpy_Destroyed => ("Drone_lumpy_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "lumpy"
            });
            public static (string, StoryNode) Drone_lumpy_Spawned => ("Drone_lumpy_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "lumpy"
            });
            public static (string, StoryNode) Drone_morton_Destroyed => ("Drone_morton_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "morton"
            });
            public static (string, StoryNode) Drone_morton_Spawned => ("Drone_morton_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "morton"
            });
            public static (string, StoryNode) Drone_namelessone_Destroyed => ("Drone_namelessone_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "namelessone"
            });
            public static (string, StoryNode) Drone_namelessone_Spawned => ("Drone_namelessone_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "namelessone"
            });
            public static (string, StoryNode) Drone_numberone_Destroyed => ("Drone_numberone_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "numberone"
            });
            public static (string, StoryNode) Drone_numberone_Spawned => ("Drone_numberone_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "numberone"
            });
            public static (string, StoryNode) Drone_roy_Destroyed => ("Drone_roy_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "roy"
            });
            public static (string, StoryNode) Drone_roy_Spawned => ("Drone_roy_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "roy"
            });
            public static (string, StoryNode) Drone_scoobert_Destroyed => ("Drone_scoobert_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "scoobert"
            });
            public static (string, StoryNode) Drone_scoobert_Spawned => ("Drone_scoobert_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "scoobert"
            });
            public static (string, StoryNode) Drone_sparky_Destroyed => ("Drone_sparky_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "sparky"
            });
            public static (string, StoryNode) Drone_sparky_Spawned => ("Drone_sparky_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "sparky"
            });
            public static (string, StoryNode) Drone_stinky_Destroyed => ("Drone_stinky_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "stinky"
            });
            public static (string, StoryNode) Drone_stinky_Spawned => ("Drone_stinky_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "stinky"
            });
            public static (string, StoryNode) Drone_wendy_Destroyed => ("Drone_wendy_Destroyed", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneDestroyed = "wendy"
            });
            public static (string, StoryNode) Drone_wendy_Spawned => ("Drone_wendy_Spawned", new StoryNode()
            {
                type = NodeType.combat,
                oncePerRun = true,
                allPresent = [ "goat" ],
                priority = true,
                lastNamedDroneSpawned = "wendy"
            });
            public static (string, StoryNode) EuniceHitOncePerRun => ("EuniceHitOncePerRun", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustHit = true,
                oncePerRun = true,
                allPresent = [ "eunice" ]
            });
            public static (string, StoryNode) EuniceMiss => ("EuniceMiss", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustMissed = true,
                oncePerCombat = true,
                doesNotHaveArtifacts = [ "Recalibrator", "GrazerBeam" ],
                allPresent = [ "eunice" ]
            });
            public static (string, StoryNode) EvilRiggsWhenRiggsTriesToLeave => ("EvilRiggsWhenRiggsTriesToLeave", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "pirateBoss" ],
                enemyIntent = "hardRiggsGoesAggro",
                oncePerCombat = true,
                priority = true
            });
            public static (string, StoryNode) Finale_Dizzy_2_PostShout => ("Finale_Dizzy_2_PostShout", new StoryNode()
            {
                type = NodeType.combat,
                oncePerCombat = true,
                priority = true,
                specialFight = "finale",
                enemyIntent = "saveTheCrewShout",
                allPresent = [ "comp" ]
            });
            public static (string, StoryNode) IsaacHasTooMuchRockFactoryButYouHaveGravelRecycler => ("IsaacHasTooMuchRockFactoryButYouHaveGravelRecycler", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                lookup = [ "RockFactoryTooMany" ],
                oncePerCombatTags = [ "RockFactoryTooManyRecycle" ],
                oncePerRun = true,
                hasArtifacts = [ "GravelRecycler" ],
                allPresent = [ "goat" ]
            });
            public static (string, StoryNode) IsaacHasTooMuchRockFactoryButYouHaveSalvageArm => ("IsaacHasTooMuchRockFactoryButYouHaveSalvageArm", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                lookup = [ "RockFactoryTooMany" ],
                oncePerCombatTags = [ "RockFactoryTooMany" ],
                oncePerRun = true,
                hasArtifacts = [ "SalvageArm" ],
                allPresent = [ "goat" ]
            });
            public static (string, StoryNode) IsaacHasTooMuchRockFactory => ("IsaacHasTooMuchRockFactory", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                lookup = [ "RockFactoryTooMany" ],
                oncePerCombatTags = [ "RockFactoryTooMany" ],
                oncePerRun = true,
                doesNotHaveArtifacts = [ "SalvageArm", "GravelRecycler" ],
                allPresent = [ "goat" ]
            });
            public static (string, StoryNode) JustPlayedAnEphemeralCard => ("JustPlayedAnEphemeralCard", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                whoDidThat = Deck.ephemeral,
                oncePerRunTags = [ "usedAnEphemeralCard" ],
            });
            public static (string, StoryNode) MaxIsSuperIntoTheBandit => ("MaxIsSuperIntoTheBandit", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "bandit", "hacker" ],
                priority = true,
                oncePerCombatTags = [ "MaxBanditCrush" ]
            });
            public static (string, StoryNode) MinerOncePerFightShouts => ("MinerOncePerFightShouts", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "miner" ],
                enemyIntent = "wideBigAttack",
                oncePerCombatTags = [ "MinerGonnaSmackYa4X" ]
            });
            public static (string, StoryNode) OldSpikeChattyPostRenameGeorge => ("OldSpikeChattyPostRenameGeorge", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "spike", "eunice" ],
                oncePerCombatTags = [ "OldSpikeNewName" ],
                maxTurnsThisCombat = 1,
                spikeName = "george"
            });
            public static (string, StoryNode) OopsBackwardsJupiter => ("OopsBackwardsJupiter", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                oncePerRun = true,
                oncePerCombatTags = [ "backwardsJupiterDrone" ],
                anyDronesHostile = [ "cannonDrone" ],
            });
            public static (string, StoryNode) PirateBoss_1_1 => ("PirateBoss_1_1", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "pirateBoss", "riggs" ],
                requiredScenes = [ "PirateBoss_1" ],
                excludedScenes = [ "PirateBoss_2" ]
            });
            public static (string, StoryNode) PirateGeneralShouts => ("PirateGeneralShouts", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                allPresent = [ "pirate" ],
                enemyIntent = "tauntPeri"
            });
            public static (string, StoryNode) RiggsHandCannon => ("RiggsHandCannon", new StoryNode()
            {
                type = NodeType.combat,
                priority = true,
                lookup = [ "riggsHandCannon" ],
                playerShotJustHit = true,
                oncePerCombatTags = [ "HandCannon" ],
                oncePerRun = true,
                allPresent = [ "riggs" ]
            });
            public static (string, StoryNode) SogginsEscapeIntent_2 => ("SogginsEscapeIntent_2", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                enemyIntent = "sogginsEscapeIntentPlease",
                turnStart = true,
                priority = true,
                oncePerRun = true,
                specialFight = "sogginsMissileEvent",
                requiredScenes = [ "SogginsEscapeIntent_1" ]
            });
            public static (string, StoryNode) StoneNervous => ("StoneNervous", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "stone" ],
                lookup = [ "stoneNervous" ]
            });
            public static (string, StoryNode) StoneReverse => ("StoneReverse", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "stone" ],
                lookup = [ "stoneReverse" ]
            });
            public static (string, StoryNode) StrafeMissed => ("StrafeMissed", new StoryNode()
            {
                type = NodeType.combat,
                playerShotJustMissed = true,
                playerShotWasFromStrafe = true,
                oncePerCombat = true,
                doesNotHaveArtifacts = [ "Recalibrator", "GrazerBeam" ],
            });
            public static (string, StoryNode) WeAreMovingAroundALot => ("WeAreMovingAroundALot", new StoryNode()
            {
                type = NodeType.combat,
                minMovesThisTurn = 3,
                oncePerRun = true,
            });
            public static (string, StoryNode) WeGotShard => ("WeGotShard", new StoryNode()
            {
                type = NodeType.combat,
                lastTurnPlayerStatuses = [ Status.shard ],
                oncePerCombat = true,
            });
        }

        public class Relevance0
        {
            public static (string, StoryNode) BatboyCowardice => ("BatboyCowardice", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "batboy" ],
                enemyIntent = "batboyIsACoward"
            });
            public static (string, StoryNode) ChunkThreats => ("ChunkThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "chunk" ],
                maxTurnsThisCombat = 1
            });
            public static (string, StoryNode) CrabThreats => ("CrabThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "crab" ],
                maxTurnsThisCombat = 1,
                priority = true,
                oncePerCombatTags = [ "CrabThreats" ]
            });
            public static (string, StoryNode) EvilRiggsIsSeriousYouKnow => ("EvilRiggsIsSeriousYouKnow", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "pirateBoss" ],
                enemyIntent = "hardRiggsGetsMad",
                priority = true,
                oncePerCombatTags = [ "RiggsBossGivesYouOneTurnToGetBackHere" ]
            });
            public static (string, StoryNode) MinerGeneralShouts => ("MinerGeneralShouts", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "miner" ],
                enemyIntent = "wideBigAttack",
                oncePerCombat = true
            });
            public static (string, StoryNode) OldSpikeChattyPostRenameSpikeTwo => ("OldSpikeChattyPostRenameSpikeTwo", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "spike" ],
                oncePerCombatTags = [ "OldSpikeNewName" ],
                maxTurnsThisCombat = 1,
                spikeName = "spiketwo"
            });
            public static (string, StoryNode) Pirate_1_1 => ("Pirate_1_1", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                priority = true,
                oncePerRun = true,
                allPresent = [ "pirate" ],
                requiredScenes = [ "Pirate_1" ],
                excludedScenes = [ "Pirate_2" ]
            });
            public static (string, StoryNode) SashaSportsShouts => ("SashaSportsShouts", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "sasha" ],
                oncePerCombat = true,
                playerJustShotASoccerBall = true
            });
            public static (string, StoryNode) ScrapThreats => ("ScrapThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "scrap" ],
                maxTurnsThisCombat = 1
            });
            public static (string, StoryNode) SogginsEscapeIntent_4 => ("SogginsEscapeIntent_4", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                enemyIntent = "sogginsEscapeIntentPlease",
                turnStart = true,
                priority = true,
                oncePerRun = false,
                specialFight = "sogginsMissileEvent",
                lookup = [ "sogginsPleaseLetMeLeave" ],
                requiredScenes = [ "SogginsEscapeIntent_3" ]
            });
            public static (string, StoryNode) Soggins_Missile_Shout_2 => ("Soggins_Missile_Shout_2", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true,
                requiredScenes = [ "Soggins_Missile_Shout_1" ]
            });
            public static (string, StoryNode) Soggins_Missile_Shout_3 => ("Soggins_Missile_Shout_3", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true,
                requiredScenes = [ "Soggins_Missile_Shout_2" ]
            });
            public static (string, StoryNode) Soggins_Missile_Shout_4 => ("Soggins_Missile_Shout_4", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "soggins" ],
                priority = true,
                specialFight = "sogginsMissileEvent",
                turnStart = true,
                oncePerCombat = true,
                requiredScenes = [ "Soggins_Missile_Shout_3" ]
            });
            public static (string, StoryNode) SpikeThreats => ("SpikeThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "oxygenguy" ],
                maxTurnsThisCombat = 1,
                priority = true,
                oncePerCombatTags = [ "SpikeThreats" ]
            });
            public static (string, StoryNode) StardogSeesYouFlippedHisMissiles => ("StardogSeesYouFlippedHisMissiles", new StoryNode()
            {
                type = NodeType.combat,
                allPresent = [ "wolf" ],
                oncePerCombatTags = [ "StardogIsInTroubleNow" ],
                priority = true,
                lookup = [ "flippedMissile" ]
            });
            public static (string, StoryNode) StardogThreats => ("StardogThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "wolf" ],
                maxTurnsThisCombat = 2,
                priority = true,
                oncePerCombatTags = [ "StardogThreats" ]
            });
            public static (string, StoryNode) TentacleThreatsOnShellBreak => ("TentacleThreatsOnShellBreak", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "tentacle" ],
                priority = true,
                enemyIntent = "cracked",
                oncePerRunTags = [ "youCrackedTheStarnacleShellBro" ]
            });
            public static (string, StoryNode) TentacleThreats => ("TentacleThreats", new StoryNode()
            {
                type = NodeType.combat,
                turnStart = true,
                allPresent = [ "tentacle" ],
                maxTurnsThisCombat = 1,
                priority = true
            });
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
