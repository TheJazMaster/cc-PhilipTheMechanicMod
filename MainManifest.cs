using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using PhilipTheMechanic.cards;

namespace PhilipTheMechanic
{
    public class MainManifest : ISpriteManifest, ICardManifest, ICharacterManifest, IDeckManifest, IAnimationManifest
    {
        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[0];

        public DirectoryInfo? GameRootFolder { get; set; }
        public Microsoft.Extensions.Logging.ILogger? Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }

        public string Name => "clay.PhilipTheEngineer";

        internal static Dictionary<string, ExternalSprite> sprites = new Dictionary<string, ExternalSprite>();
        internal static Dictionary<string, ExternalAnimation> animations = new Dictionary<string, ExternalAnimation>();
        internal static Dictionary<string, ExternalCard> cards = new Dictionary<string, ExternalCard>();
        internal static ExternalCharacter character;
        internal static ExternalDeck deck;

        public void BootMod(IModLoaderContact contact)
        {
            var harmony = new Harmony("PhilipTheMechanic");
            harmony.PatchAll();

            Logger?.Log("Philip Loaded!");
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
                "philip_standard_0",
                "philip_standard_1",
                "philip_surprise_0",
                "philip_surprise_1",
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
            var overdriveMod = new ExternalCard("clay.PhilipTheMechanic.cards.OverdriveMod", typeof(OverdriveMod), sprites["card_philip_default"], deck);
            overdriveMod.AddLocalisation("Overdrive Mod");
            registry.RegisterCard(overdriveMod);
            cards["Overdrive Mod"] = overdriveMod;
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
            character = new ExternalCharacter(
                "clay.PhilipTheMechanic.Philip",
                deck,
                sprites["char_frame_philip"],
                new Type[] { typeof(OverdriveMod) }, // TODO: give starting cards for Philip
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
            animationInfo["neutral"] = new ExternalSprite[] { sprites["philip_standard_0"], sprites["philip_standard_1"] };
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
    }
}
