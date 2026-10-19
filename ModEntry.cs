using clay.PhilipTheMechanic.Actions;
using clay.PhilipTheMechanic.Artifacts;
using clay.PhilipTheMechanic.Cards;
using clay.PhilipTheMechanic.Controllers;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TheJazMaster.MoreDifficulties;

/* In the Cobalt Core modding community it is common for namespaces to be <Author>.<ModName>
 * This is helpful to know at a glance what mod we're looking at, and who made it */
namespace clay.PhilipTheMechanic;

/* ModEntry is the base for our mod. Others like to name it Manifest, and some like to name it <ModName>
 * Notice the ': SimpleMod'. This means ModEntry is a subclass (child) of the superclass SimpleMod (parent). This is help us use Nickel's functions more easily! */
public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal readonly IPhilipAPI Api = new ApiImplementation();
    internal IKokoroApi KokoroApi { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }

    internal Harmony Harmony { get; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }


    internal Dictionary<string, Spr> sprites = [];

    internal IDeckEntry PhilipDeck { get; }

    internal Status RedrawStatus => KokoroApi.RedrawVanillaStatus;
    internal IStatusEntry CustomPartsStatus { get; private set; }


    public override object? GetApi(IModManifest requestingMod)
    {
        return Api;
    }

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;

        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        KokoroApi.Actions.RegisterWrappedActionHook(new KokoroHooksImplementation(), 0);
        // KokoroApi.RegisterCardRenderHook(new KokoroHooksImplementation(), 1000);

        MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");

        Harmony = new Harmony(package.Manifest.UniqueName);
        Harmony.PatchAll();

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
        );

        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) =>
        {
            // This is where hooks would trigger, if I knew how to make APIs  - Jaz
            TopMarks.CheckTrigger(state, combat, card, handPosition);
            ModifierCardsController.HandleFlimsyModifiers(card, state, combat, handPosition);

            ModifierCardsController.CalculateCardModifiers(state, combat);
        }, 0);

        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) => Nanobots.Replicate(state, combat), 0);

        // sprites
        RegisterSprites(package);

        // deck and character
        PhilipDeck = Helper.Content.Decks.RegisterDeck("PhilipDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("c9f000"),
                titleColor = new Color("000000")
            },
            DefaultCardArt = sprites["card_philip_default"],
            BorderSprite = sprites["frame_philip"],
            Name = AnyLocalizations.Bind(new List<String>() { "character", "Philip", "name" }).Localize,
        });

        Helper.Content.Characters.V2.RegisterPlayableCharacter("Philip", new PlayableCharacterConfigurationV2()
        {
            Deck = PhilipDeck.Deck,
            Starters = new StarterDeck {
                cards = [new OverdriveMod(), new TinkerShot()],
            },
            Description = AnyLocalizations.Bind(new List<String>() { "character", "Philip", "description" }).Localize,
            BorderSprite = sprites["char_frame_philip"],
            NeutralAnimation = new()
            {
                CharacterType = PhilipDeck.Deck.Key(),
                LoopTag = "neutral",
                Frames = new[] { sprites[$"philip_neutral_0"], sprites[$"philip_neutral_1"], sprites[$"philip_neutral_0"], sprites[$"philip_neutral_3"], }
            },
            MiniAnimation = new()
            {
                CharacterType = PhilipDeck.Deck.Key(),
                LoopTag = "mini",
                Frames = new[] { sprites[$"philip_mini"] }
            }
        });

        MoreDifficultiesApi?.RegisterAltStarters(PhilipDeck.Deck, new() { cards = [new ReduceReuse(), new DuctTapeAndDreams()] });

        // statuses
        CustomPartsStatus = helper.Content.Statuses.RegisterStatus("CustomParts", new()
        {
            Definition = new()
            {
                icon = sprites["icon_customParts"],
                color = new("777777"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "customParts", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "customParts", "description"]).Localize
        });

        // animations
        RegisterAnimation("classy");
        RegisterAnimation("maniacal");
        RegisterAnimation("squint");
        RegisterAnimation("surprise");
        RegisterAnimation("sheepish");
        RegisterAnimation("excited");
        RegisterSingleFrameAnimation("gameover");
        RegisterSingleFrameAnimation("proud");
        RegisterSingleFrameAnimation("whatisthat");
        RegisterSingleFrameAnimation("unhappy");
        RegisterSingleFrameAnimation("hot_chocolate");
        Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = PhilipDeck.Deck.Key(),
            LoopTag = "laugh",

            Frames = new[] { sprites["philip_laugh_0"], sprites["philip_laugh_1"], sprites["philip_laugh_0"], sprites["philip_laugh_1"], }
        });

        // register cards
        var cardTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => string.Equals(t.Namespace, "clay.PhilipTheMechanic.Cards", StringComparison.Ordinal))
            .ToList();
        foreach (var cardType in cardTypes)
        {
            if (!typeof(IRegisterableCard).IsAssignableFrom(cardType)) continue;

            helper.Content.Cards.RegisterCard(cardType.Name, new()
            {
                CardType = cardType,
                Meta = new()
                {
                    deck = PhilipDeck.Deck,
                    rarity = (Rarity)AccessTools.DeclaredMethod(cardType, nameof(IRegisterableCard.GetRarity))?.Invoke(null, [])!,
                    upgradesTo = [Upgrade.A, Upgrade.B]
                },
                Name = AnyLocalizations.Bind(["card", cardType.Name, "name"]).Localize,
                Art = (Spr)AccessTools.DeclaredMethod(cardType, nameof(IRegisterableCard.GetArt))?.Invoke(null, [])!,
            });
        }

        // register unofferable cards
        helper.Content.Cards.RegisterCard("Nanobots", new()
        {
            CardType = typeof(Nanobots),
            Meta = new()
            {
                deck = Deck.trash,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true
            },
            Name = AnyLocalizations.Bind(["card", "Nanobots", "name"]).Localize,
            Art = Instance.sprites["card_Nanobots_2"]
        });

        helper.Content.Cards.RegisterCard("ExtenderMod", new()
        {
            CardType = typeof(ExtenderMod),
            Meta = new()
            {
                deck = PhilipDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true
            },
            Name = AnyLocalizations.Bind(["card", "ExtenderMod", "name"]).Localize,
            Art = Instance.sprites["card_Uh_Oh"]
        });
        helper.Content.Cards.RegisterCard("UraniumRound", new()
        {
            CardType = typeof(UraniumRound),
            Meta = new()
            {
                deck = PhilipDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true
            },
            Name = AnyLocalizations.Bind(["card", "UraniumRound", "name"]).Localize,
            Art = Instance.sprites["card_Uranium_Rounds"]
        });
        helper.Content.Cards.RegisterCard("ImpromptuBlastShield", new()
        {
            CardType = typeof(ImpromptuBlastShield),
            Meta = new()
            {
                deck = PhilipDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true
            },
            Name = AnyLocalizations.Bind(["card", "ImpromptuBlastShield", "name"]).Localize,
            Art = Instance.sprites["card_Impromptu_Blast_Shield"]
        });
        helper.Content.Cards.RegisterCard("OhNo", new()
        {
            CardType = typeof(OhNo),
            Meta = new()
            {
                deck = PhilipDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B],
                dontOffer = true
            },
            Name = AnyLocalizations.Bind(["card", "OhNo", "name"]).Localize,
            Art = Instance.sprites["card_Uh_Oh"]
        });

        // register artifacts
        var artifactTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => String.Equals(t.Namespace, "clay.PhilipTheMechanic.Artifacts", StringComparison.Ordinal))
            .ToList();
        foreach (var artifactType in artifactTypes)
        {
            if (!typeof(IRegisterableArtifact).IsAssignableFrom(artifactType)) continue;

            helper.Content.Artifacts.RegisterArtifact(artifactType.Name, new()
            {
                ArtifactType = artifactType,
                Meta = new()
                {
                    owner = PhilipDeck.Deck,
                    pools = (ArtifactPool[])AccessTools.DeclaredMethod(artifactType, nameof(IRegisterableArtifact.GetPools))?.Invoke(null, [])!
                },
                Sprite = (Spr)AccessTools.DeclaredMethod(artifactType, nameof(IRegisterableArtifact.GetSpriteForRegistering))?.Invoke(null, [])!,
                Name = AnyLocalizations.Bind(["artifact", artifactType.Name, "name"]).Localize,
                Description = AnyLocalizations.Bind(["artifact", artifactType.Name, "description"]).Localize
            });
        }

        KokoroApi.RegisterRedrawStatusHook(new EndlessToolboxHook(), 0);
        // KokoroApi.RegisterRedrawStatusHook(new HotChocolateHook(), 1);
        // KokoroApi.RegisterRedrawStatusHook(new ScrapMagnetHook(), 2);

        //
        // Register Dialogue
        //

        DialogueRegistration.LoadAll();
    }

    private void RegisterSprites(IPluginPackage<IModManifest> package)
    {
        var files = package.PackageRoot.GetRelativeDirectory("assets").AsDirectory?.Files.Where(f => f.Name.EndsWith(".png"));
        if (files == null) {
            Logger.LogError("Could not find assets folder! Philip the Mechanic will not work");
            throw new Exception();
        }
        foreach (IFileInfo file in files) {
            sprites[file.Name[..(file.Name.Length - 4)]] = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/{file.Name}")).Sprite;
        }
    }

    private ICharacterAnimationEntryV2 RegisterAnimation(string looptag)
    {
        return Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = PhilipDeck.Deck.Key(),
            LoopTag = looptag,

            Frames = new[] { sprites[$"philip_{looptag}_0"], sprites[$"philip_{looptag}_1"], sprites[$"philip_{looptag}_0"], sprites[$"philip_{looptag}_3"], }
        });
    }
    private void RegisterSingleFrameAnimation(string looptag)
    {
        Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
        {
            CharacterType = PhilipDeck.Deck.Key(),
            LoopTag = looptag,

            Frames = new[] { sprites[$"philip_{looptag}"] }
        });
    }
}
