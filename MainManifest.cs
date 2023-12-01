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
using PhilipTheMechanic.cards;

namespace PhilipTheMechanic
{
    public class MainManifest : IModManifest, ISpriteManifest, ICardManifest, ICharacterManifest, IDeckManifest, IAnimationManifest, IGlossaryManifest
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
                "card_philip_default",
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
                "icon_2x_sticker",
                "icon_sticker_add_card",
                "icon_sticker_buff_attack",
                "icon_sticker_hull_damage",
                "icon_sticker_energy_discount",
                "icon_sticker_0_energy",
                "icon_sticker_attack",
                "icon_sticker_temp_shield_attack",
                "icon_sticker_shield_attack",
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
            var cardDefinitions = new ExternalCard[]
            {
                new ExternalCard("clay.PhilipTheMechanic.cards.Overdrive Mod", typeof(OverdriveMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Frenzy Mod", typeof(FrenzyMod), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Loosen Screws", typeof(LoosenScrews), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Overfueled Engines", typeof(OverfueledEngines), sprites["card_philip_default"], deck),
                new ExternalCard("clay.PhilipTheMechanic.cards.Shielding Mod", typeof(ShieldingMod), sprites["card_philip_default"], deck),
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
            //var realStartingCards = new Type[] { typeof(OverdriveMod), typeof(FrenzyMod) };
            var realStartingCards = new Type[] { typeof(OverdriveMod) }; // add a redraw card

            character = new ExternalCharacter(
                "clay.PhilipTheMechanic.Philip",
                deck,
                sprites["char_frame_philip"],
                new Type[] { typeof(OverdriveMod), typeof(LoosenScrews), typeof(FrenzyMod), typeof(OverfueledEngines), typeof(ShieldingMod) }, // TODO: give starting cards for Philip
                new Type[0],
                animations["neutral"],
                animations["mini"]
            );

            character.AddNameLocalisation("Philip");
            character.AddDescLocalisation("<c=c9f000>PHILIP</c>\nYour mechanic. His cards modify other cards in your hand, and are typically unplayable.");
            // TODO: update desc
            // His cards modify other cards in your hand, provide redraw, or modify your ship.

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
                "Increases the power of attacks on the target card."
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
    }
}
