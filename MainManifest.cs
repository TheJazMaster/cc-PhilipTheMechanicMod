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
        public static ExternalCharacter character;
        public static ExternalDeck deck;

        public void BootMod(IModLoaderContact contact)
        {
            Instance = this;
            var harmony = new Harmony("PhilipTheMechanic");
            harmony.PatchAll();

            Logger.LogCritical("I'm still directly referencing enums - make sure to use that reflection method to reference them instead before publishing this mod!");
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
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
                "icon_play_twice"
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
            var realStartingCards = new Type[] { typeof(OverdriveMod), typeof(FrenzyMod) };

            character = new ExternalCharacter(
                "clay.PhilipTheMechanic.Philip",
                deck,
                sprites["char_frame_philip"],
                new Type[] { typeof(OverdriveMod), typeof(LoosenScrews), typeof(FrenzyMod), typeof(OverfueledEngines) }, // TODO: give starting cards for Philip
                new Type[0],
                animations["neutral"],
                animations["mini"]
            );

            character.AddNameLocalisation("Philip");
            character.AddDescLocalisation("<c=c9f000>PHILIP</c>\nYour mechanic. His cards modify other cards in your hand, and are typically unplayable.");

            if (!registry.RegisterCharacter(character)) throw new Exception("Philip is lost! Could not register Philip!");
        }

        public void LoadManifest(IAnimationRegistry registry)
        {
            var animationInfo = new Dictionary<string, IEnumerable<ExternalSprite>>();
            animationInfo["neutral"] = new ExternalSprite[] { sprites["philip_neutral_0"], sprites["philip_neutral_1"] };
            animationInfo["squint"] = new ExternalSprite[] { sprites["philip_squint_0"], sprites["philip_squint_1"] };
            animationInfo["gameover"] = new ExternalSprite[] { sprites["philip_surprise_0"], sprites["philip_surprise_1"] };
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
            var aReplayGlossary = new ExternalGlossary("clay.PhilipTheMechanic.Glossary", "AReplay", false, ExternalGlossary.GlossayType.action, sprites["icon_play_twice"]);
            aReplayGlossary.AddLocalisation("en", "play twice", "Play all actions prior to the Play Twice action twice.");
            registry.RegisterGlossary(aReplayGlossary);
            glossary["AReplay"] = aReplayGlossary;
        }
    }
}
