using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;
using PhilipTheMechanic.artifacts;
using PhilipTheMechanic.cards;

namespace PhilipTheMechanic
{
    public class MainManifest : IModManifest, ISpriteManifest, ICardManifest, ICharacterManifest, IDeckManifest, IAnimationManifest, IGlossaryManifest, IStatusManifest, IArtifactManifest
    {
        public static MainManifest Instance;

        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[0];

        public DirectoryInfo? GameRootFolder { get; set; }
        public Microsoft.Extensions.Logging.ILogger? Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }

        public string Name => "clay.PhilipTheEngineer";

        public static Dictionary<string, ExternalSprite> sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalAnimation> animations = new Dictionary<string, ExternalAnimation>();
        public static Dictionary<string, ExternalCard> cards = new Dictionary<string, ExternalCard>();
        public static Dictionary<string, ExternalStatus> statuses = new Dictionary<string, ExternalStatus>();
        public static Dictionary<string, ExternalGlossary> glossary = new Dictionary<string, ExternalGlossary>();
        public static Dictionary<string, CustomTTGlossary> vanillaSpritesGlossary = new Dictionary<string, CustomTTGlossary>();
        public static ExternalCharacter character;
        public static ExternalDeck deck;

        public void BootMod(IModLoaderContact contact)
        {
            Instance = this;
            var harmony = new Harmony("PhilipTheMechanic");
            harmony.PatchAll();
            CustomTTGlossary.Apply(harmony);

            Logger.LogCritical("I'm still directly referencing enums - make sure to use that reflection method to reference them instead before publishing this mod!");
            // List of enums still used: Upgrade
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            // TODO: explore sprites folder instead of manually listing the contents
            var filenames = new string[] { 
                "char_frame_philip", 
                "frame_philip",

                "philip_classy",
                "philip_maniacal",
                "philip_mini",
                "philip_squint_0",
                "philip_squint_1",
                "philip_neutral_0",
                "philip_neutral_1",
                "philip_surprise_0",
                "philip_surprise_1",

                "icon_play_twice",
                "icon_all_cards_to_the_left",
                "icon_all_cards_to_the_right",
                "icon_card_to_the_left",
                "icon_card_to_the_right",
                "icon_attack_buff",
                "icon_screw",
                "icon_equal",
                "icon_redraw",
                "icon_customParts",
                "icon_no_action",

                "icon_2x_sticker",
                "icon_sticker_add_card",
                "icon_sticker_buff_attack",
                "icon_sticker_hull_damage",
                "icon_sticker_energy_discount",
                "icon_sticker_0_energy",
                "icon_sticker_attack",
                "icon_sticker_temp_shield_attack",
                "icon_sticker_shield_attack",
                "icon_sticker_temp_shield",
                "icon_sticker_shield",
                "icon_sticker_piercing",
                "icon_sticker_heat",
                "icon_sticker_evade",
                "icon_sticker_exhaust",
                "icon_sticker_missile",
                "icon_sticker_hermes",
                "icon_sticker_stun",
                "icon_sticker_recycle",
                "icon_sticker_no_action",

                "button_redraw",
                "button_redraw_on",

                "artifact_wire_clippers",
                "artifact_sturdy_pliers",
                "artifact_endless_toolbox",

                "card_philip_default",
                "card_Black_Market_Parts"
            };

            foreach (var filename in filenames) {
                var filepath = Path.Combine(ModRootFolder?.FullName ?? "", "sprites", filename+".png");
                var sprite = new ExternalSprite("clay.PhilipTheEngineer.sprites."+filename, new FileInfo(filepath));
                sprites[filename] = sprite;

                if (!artRegistry.RegisterArt(sprite)) throw new Exception("Error registering sprite " + filename);
            }
        }

        public void LoadManifest(ICardRegistry registry)
        {
            // GOAL:
            // 21 cards
            // 9 common, 7 uncommon, 5 rare
            var cardDefinitions = new ExternalCard[]
            {
                new ExternalCard("clay.PhilipTheMechanic.cards.Overdrive Mod", typeof(OverdriveMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Frenzy Mod", typeof(FrenzyMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Loosen Screws", typeof(LoosenScrews), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Overfueled Engines", typeof(OverfueledEngines), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Shielding Mod", typeof(ShieldingMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Recycle Parts", typeof(RecycleParts), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Emergency Training", typeof(EmergencyTraining), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Impromptu Blast Shield", typeof(ImpromptuBlastShield), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Piercing Mod", typeof(PiercingMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Permanence Mod", typeof(PermanenceMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Disable Safties", typeof(DisableSafties), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Overheated Cannons", typeof(OverheatedCannons), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.No Stock Parts", typeof(NoStockParts), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Black Market Parts", typeof(BlackMarketParts), sprites["card_Black_Market_Parts"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Duct Tape and Dreams", typeof(DuctTapeAndDreams), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Jettison Parts", typeof(JettisonParts), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Nanobot Infestation", typeof(NanobotInfestation), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Nanobots", typeof(Nanobots), sprites["card_philip_default"], ExternalDeck.GetRaw((int)Enum.Parse<Deck>("trash"))), 
                new ExternalCard("clay.PhilipTheMechanic.cards.Oh No", typeof(OhNo), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Oops", typeof(Oops), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Open Bay Doors", typeof(OpenBayDoors), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Plan WAF", typeof(PlanWAF), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Precision Machining", typeof(PrecisionMachining), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Reduce Reuse", typeof(ReduceReuse), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Stun Mod", typeof(StunMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Uranium Round", typeof(UraniumRound), sprites["card_philip_default"], deck),
            };

            foreach(var card in cardDefinitions)
            {
                var name = card.GlobalName.Split('.').LastOrDefault() ?? "FAILED TO FIND NAME";
                card.AddLocalisation(name);
                registry.RegisterCard(card);
                cards[name] = card;
            }
        }

        public void LoadManifest(IDeckRegistry registry)
        {
            var philipColor = 0;
            unchecked { philipColor = (int)0xffc9f000; }

            deck = new ExternalDeck(
                "clay.PhilipTheMechanic.PhilipDeck",
                System.Drawing.Color.FromArgb(philipColor),
                System.Drawing.Color.Black,
                sprites["card_philip_default"],
                sprites["frame_philip"],
                null
            );
            if (!registry.RegisterDeck(deck)) throw new Exception("Philip's lost his deck. Cannot proceed until he finds it.");
        }

        public void LoadManifest(ICharacterRegistry registry)
        {
            var realStartingCards = new Type[] { typeof(OverdriveMod), typeof(RecycleParts) };
            //var testStartCards = cards.Values.Select(card => card.CardType).ToList();
            //testStartCards.Add(typeof(UraniumRound));
            //testStartCards.Add(typeof(OverheatedCannons));

            character = new ExternalCharacter(
                "clay.PhilipTheMechanic.Philip",
                deck,
                sprites["char_frame_philip"],
                realStartingCards,
                new Type[0],
                animations["neutral"],
                animations["mini"]
            );

            character.AddNameLocalisation("Philip");
            character.AddDescLocalisation("<c=c9f000>PHILIP</c>\nYour ship engineering officer. His cards modify other cards in your hand, provide <c=d6525f>redraw</c>, and are often unplayable.");

            if (!registry.RegisterCharacter(character)) throw new Exception("Philip is lost! Could not register Philip!");
        }

        public void LoadManifest(IAnimationRegistry registry)
        {
            var animationInfo = new Dictionary<string, IEnumerable<ExternalSprite>>();
            animationInfo["neutral"] = new ExternalSprite[] { sprites["philip_neutral_0"], sprites["philip_neutral_1"], sprites["philip_neutral_0"], sprites["philip_neutral_1"] };
            animationInfo["squint"] = new ExternalSprite[] { sprites["philip_squint_0"], sprites["philip_squint_1"], sprites["philip_squint_0"], sprites["philip_squint_1"] };
            animationInfo["gameover"] = new ExternalSprite[] { sprites["philip_surprise_0"], sprites["philip_surprise_1"], sprites["philip_surprise_0"], sprites["philip_surprise_1"] };
            animationInfo["mini"] = new ExternalSprite[] { sprites["philip_mini"] };

            foreach (var kvp in animationInfo)
            {
                var animation = new ExternalAnimation(
                    "clay.PhilipTheMechanic.animations."+kvp.Key,
                    deck,
                    kvp.Key,
                    false,
                    kvp.Value
                );
                animations[kvp.Key] = animation;

                if (!registry.RegisterAnimation(animation)) throw new Exception("Error registering animation " + kvp.Key);
            }
        }

        public void LoadManifest(IGlossaryRegisty registry)
        {
            RegisterGlossaryEntry(registry, "AReplay", sprites["icon_play_twice"],
                "play twice",
                "Play all actions prior to the Play Twice action twice."
            );

            RegisterGlossaryEntry(registry, "ACardToTheLeft", sprites["icon_card_to_the_left"],
                "modify card to the left",
                "Add the following effects to the card to the left. They do NOT trigger when this card is played."
            );

            RegisterGlossaryEntry(registry, "AAllCardsToTheLeft", sprites["icon_all_cards_to_the_left"],
                "modify all cards to the left",
                "Add the following effects to all cards to the left. They do NOT trigger when this card is played."
            );

            RegisterGlossaryEntry(registry, "ACardToTheRight", sprites["icon_card_to_the_right"],
                "modify card to the right",
                "Add the following effects to the card to the right. They do NOT trigger when this card is played."
            );

            RegisterGlossaryEntry(registry, "AAllCardsToTheRight", sprites["icon_all_cards_to_the_right"],
                "modify all cards to the right",
                "Add the following effects to all cards to the right. They do NOT trigger when this card is played."
            );

            RegisterGlossaryEntry(registry, "AAttackBuff", sprites["icon_attack_buff"],
                "attack buff",
                "Increases the power of attacks on the target card by {0}."
            );

            RegisterGlossaryEntry(registry, "SRedraw", sprites["icon_redraw"],
                "redraw",
                "Allows you to discard {0} cards and draw a new one for each discarded."
            );

            RegisterGlossaryEntry(registry, "ANoAction", sprites["icon_no_action"],
                "no action",
                "All effects of the target card are erased."
            );

            vanillaSpritesGlossary["AEnergyDiscount"] = new CustomTTGlossary(CustomTTGlossary.GlossaryType.cardtrait, Enum.Parse<Spr>("icons_discount"), "energy discount", "Discounts the energy cost of this card.", null);
            vanillaSpritesGlossary["ASetEnergy"] = new CustomTTGlossary(CustomTTGlossary.GlossaryType.cardtrait, Enum.Parse<Spr>("icons_energy"), "set energy cost", "Changes the energy cost of this card. Overrides all other effects.", null);
        }
        private void RegisterGlossaryEntry(IGlossaryRegisty registry, string itemName, ExternalSprite sprite, string displayName, string description)
        {
            var entry = new ExternalGlossary("clay.PhilipTheMechanic.Glossary", itemName, false, ExternalGlossary.GlossayType.action, sprite);
            entry.AddLocalisation("en", displayName, description);
            registry.RegisterGlossary(entry);
            glossary[entry.ItemName] = entry;
        }

        public void LoadManifest(IStatusRegistry statusRegistry)
        {
            var redraw = new ExternalStatus("clay.PhilipTheMechanic.Statuses.Redraw", true, System.Drawing.Color.Red, null, sprites["icon_redraw"], false);
            statusRegistry.RegisterStatus(redraw);
            redraw.AddLocalisation("Redraw", "Enables you to discard a card of your choice and draw a new one. You may do this up to {0} times.");
            statuses["redraw"] = redraw;

            var customParts = new ExternalStatus("clay.PhilipTheMechanic.Statuses.CustomParts", true, System.Drawing.Color.Red, null, sprites["icon_customParts"], false);
            statusRegistry.RegisterStatus(customParts);
            customParts.AddLocalisation("Custom Parts", "Gives you {0} redraw at the start of each turn.");
            statuses["customParts"] = customParts;
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            var wireClippers = new ExternalArtifact("clay.PhilipTheMechanic.Artifacts.WireClippers", typeof(WireClippers), sprites["artifact_wire_clippers"], ownerDeck: deck);
            wireClippers.AddLocalisation("WIRE CLIPPERS", "All unplayable cards become playable");
            registry.RegisterArtifact(wireClippers);

            var sturdyPliers = new ExternalArtifact("clay.PhilipTheMechanic.Artifacts.SturdyPliers", typeof(SturdyPliers), sprites["artifact_sturdy_pliers"], ownerDeck: deck);
            sturdyPliers.AddLocalisation("STURDY PLIERS", "Gain 3 redraw at the start of combat");
            registry.RegisterArtifact(sturdyPliers);

            var endlessToolbox = new ExternalArtifact("clay.PhilipTheMechanic.Artifacts.EndlessToolbox", typeof(EndlessToolbox), sprites["artifact_endless_toolbox"], ownerDeck: deck);
            endlessToolbox.AddLocalisation("ENDLESS TOOLBOX", "When you redraw, draw an extra card");
            registry.RegisterArtifact(endlessToolbox);
        }
    }
}
