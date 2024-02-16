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
using Shockah.Kokoro;
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
    internal IKokoroApi? KokoroApi { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }

    internal Harmony Harmony { get; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }


    internal Dictionary<string, ISpriteEntry> sprites = new();

    internal IDeckEntry PhilipDeck { get; }

    internal IStatusEntry RedrawStatus { get; private set; }
    internal IStatusEntry CustomPartsStatus { get; private set; }


    public override object? GetApi(IModManifest requestingMod)
    {
        return Api;
    }

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;

        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro");
        KokoroApi?.Actions.RegisterWrappedActionHook(new KokoroHooksImplementation(), 0);
        KokoroApi?.RegisterCardRenderHook(new KokoroHooksImplementation(), 1000);

        MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");

        Harmony = new Harmony(package.Manifest.UniqueName);
        Harmony.PatchAll();

        this.AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
        );

        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) =>
        {
            ModifierCardsController.HandleFlimsyModifiers(card, state, combat, handPosition);
        }, 0);
        
        helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnTurnStart), (State state, Combat combat) => Nanobots.Replicate(state, combat), 0);

        // sprites
        RegisterSprite(package, "char_frame_philip");                
        RegisterSprite(package, "frame_philip");
        RegisterSprite(package, "floppable_fix_sticky_note");
        RegisterSprite(package, "floppable_fix_index_card");
        RegisterSprite(package, "card_art_override");
            
        RegisterSprite(package, "philip_mini");
        RegisterSprite(package, "philip_classy_0");
        RegisterSprite(package, "philip_classy_1");
        RegisterSprite(package, "philip_classy_3");
        RegisterSprite(package, "philip_maniacal_0");
        RegisterSprite(package, "philip_maniacal_1");
        RegisterSprite(package, "philip_maniacal_3");
        RegisterSprite(package, "philip_squint_0");
        RegisterSprite(package, "philip_squint_1");
        RegisterSprite(package, "philip_squint_3");
        RegisterSprite(package, "philip_neutral_0");
        RegisterSprite(package, "philip_neutral_1");
        RegisterSprite(package, "philip_neutral_3");
        RegisterSprite(package, "philip_surprise_0");
        RegisterSprite(package, "philip_surprise_1");
        RegisterSprite(package, "philip_surprise_3");
        RegisterSprite(package, "philip_sheepish_0");
        RegisterSprite(package, "philip_sheepish_1");
        RegisterSprite(package, "philip_sheepish_3");
        RegisterSprite(package, "philip_proud");
        RegisterSprite(package, "philip_whatisthat");
        RegisterSprite(package, "philip_unhappy");
        RegisterSprite(package, "philip_excited_0");
        RegisterSprite(package, "philip_excited_1");
        RegisterSprite(package, "philip_excited_3");
        RegisterSprite(package, "philip_laugh_0");
        RegisterSprite(package, "philip_laugh_1");
        RegisterSprite(package, "philip_hot_chocolate");

        RegisterSprite(package, "icon_addUpgradedCard");
        RegisterSprite(package, "icon_play_twice");
        RegisterSprite(package, "icon_all_cards_to_the_left");
        RegisterSprite(package, "icon_all_cards_to_the_right");
        RegisterSprite(package, "icon_card_to_the_left");
        RegisterSprite(package, "icon_card_to_the_right");
        RegisterSprite(package, "icon_card_neighbors");
        RegisterSprite(package, "icon_Flimsy_Left_Card_Mod");
        RegisterSprite(package, "icon_Flimsy_Right_Card_Mod");
        RegisterSprite(package, "icon_Flimsy_All_Right_Card_Mod");
        RegisterSprite(package, "icon_Flimsy_All_Left_Card_Mod");
        RegisterSprite(package, "icon_Flimsy_Neighbors_Card_Mod");
        RegisterSprite(package, "icon_attack_buff");
        RegisterSprite(package, "icon_card_unplayable");
        RegisterSprite(package, "icon_card_playable");
        RegisterSprite(package, "icon_dont_exhaust");
        RegisterSprite(package, "icon_0_energy");
        RegisterSprite(package, "icon_screw");
        RegisterSprite(package, "icon_equal");
        RegisterSprite(package, "icon_redraw");
        RegisterSprite(package, "icon_customParts");
        RegisterSprite(package, "icon_no_action");
        RegisterSprite(package, "icon_shield_x");
        RegisterSprite(package, "icon_temp_shield_x");
        RegisterSprite(package, "icon_card_is_centered");
        RegisterSprite(package, "icon_card_is_not_centered");
        RegisterSprite(package, "icon_card_is_centered_disabled");
        RegisterSprite(package, "icon_card_is_not_centered_disabled");
        RegisterSprite(package, "icon_flimsy");
        RegisterSprite(package, "icon_nanobots");
    
        RegisterSprite(package, "icon_2x_sticker");
        RegisterSprite(package, "icon_sticker_add_card");
        RegisterSprite(package, "icon_sticker_buff_attack");
        RegisterSprite(package, "icon_sticker_hull_damage");
        RegisterSprite(package, "icon_sticker_energy_discount");
        RegisterSprite(package, "icon_sticker_0_energy");
        RegisterSprite(package, "icon_sticker_attack");
        RegisterSprite(package, "icon_sticker_temp_shield_attack");
        RegisterSprite(package, "icon_sticker_shield_attack");
        RegisterSprite(package, "icon_sticker_temp_shield");
        RegisterSprite(package, "icon_sticker_shield");
        RegisterSprite(package, "icon_sticker_piercing");
        RegisterSprite(package, "icon_sticker_heat");
        RegisterSprite(package, "icon_sticker_evade");
        RegisterSprite(package, "icon_sticker_exhaust");
        RegisterSprite(package, "icon_sticker_dont_exhaust");
        RegisterSprite(package, "icon_sticker_missile");
        RegisterSprite(package, "icon_sticker_hermes");
        RegisterSprite(package, "icon_sticker_stun");
        RegisterSprite(package, "icon_sticker_recycle");
        RegisterSprite(package, "icon_sticker_redraw");
        RegisterSprite(package, "icon_sticker_retain");
        RegisterSprite(package, "icon_sticker_no_action");
        RegisterSprite(package, "icon_sticker_energyLessNextTurn");
        RegisterSprite(package, "icon_sticker_drawLessNextTurn");
        RegisterSprite(package, "icon_sticker_card_unplayable");
        RegisterSprite(package, "icon_sticker_card_playable");

        RegisterSprite(package, "button_redraw");
        RegisterSprite(package, "button_redraw_on");
    
        RegisterSprite(package, "artifact_wire_clippers");
        RegisterSprite(package, "artifact_angle_grinder");
        RegisterSprite(package, "artifact_endless_toolbox");
        RegisterSprite(package, "artifact_self_propelling_cannons");
        RegisterSprite(package, "artifact_hot_chocolate");
        RegisterSprite(package, "artifact_electromagnet");
        
        RegisterSprite(package, "card_philip_default");
        RegisterSprite(package, "card_Black_Market_Parts");
        RegisterSprite(package, "card_Last_Resort");
        RegisterSprite(package, "card_Overdrive_Mod");
        RegisterSprite(package, "card_Precise_Machining");
        RegisterSprite(package, "card_Oops");
        RegisterSprite(package, "card_Oh_No");
        RegisterSprite(package, "card_Uh_Oh");
        RegisterSprite(package, "card_Piercing_Mod");
        RegisterSprite(package, "card_Uranium_Rounds");
        RegisterSprite(package, "card_Impromptu_Blast_Shield");
        RegisterSprite(package, "card_Duct_Tape_and_Dreams");
        RegisterSprite(package, "card_Frenzy_Mod");
        RegisterSprite(package, "card_Recycle_Parts");
        RegisterSprite(package, "card_Stun_Mod");
        RegisterSprite(package, "card_Shielding_Mod");
        RegisterSprite(package, "card_Permanence_Mod");
        RegisterSprite(package, "card_Nanobot_Infestation");
        RegisterSprite(package, "card_Nanobots");
        RegisterSprite(package, "card_Nanobots_2");
        RegisterSprite(package, "card_Emergency_Training");
        RegisterSprite(package, "card_Loosen_Screws");
        RegisterSprite(package, "card_Overfueled_Engines");
        RegisterSprite(package, "card_Open_Bay_Doors");

        // deck and character
        PhilipDeck = Helper.Content.Decks.RegisterDeck("PhilipDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("c9f000"),
                titleColor = new Color("000000")
            },
            DefaultCardArt = sprites["card_philip_default"].Sprite,
            BorderSprite = sprites["frame_philip"].Sprite,
            Name = this.AnyLocalizations.Bind(new List<String>() { "character", "Philip", "name" }).Localize,
        });

        Helper.Content.Characters.RegisterCharacter("Philip", new CharacterConfiguration()
        {
            Deck = PhilipDeck.Deck,
            StarterCardTypes = new List<Type>() { typeof(OverdriveMod), typeof(RecycleParts) },
            Description = this.AnyLocalizations.Bind(new List<String>() { "character", "Philip", "description" }).Localize,
            BorderSprite = sprites["char_frame_philip"].Sprite,
            NeutralAnimation = new() {
                Deck = PhilipDeck.Deck,
                LoopTag = "neutral",
                Frames = new[] { sprites[$"philip_neutral_0"].Sprite, sprites[$"philip_neutral_1"].Sprite, sprites[$"philip_neutral_0"].Sprite, sprites[$"philip_neutral_3"].Sprite, }
            },
            MiniAnimation = new()
            {
                Deck = PhilipDeck.Deck,
                LoopTag = "mini",
                Frames = new[] { sprites[$"philip_mini"].Sprite }
            }
        });

        MoreDifficultiesApi?.RegisterAltStarters(PhilipDeck.Deck, new() { cards = new() { new ReduceReuse(), new DuctTapeAndDreams() } });

        // statuses
        RedrawStatus = helper.Content.Statuses.RegisterStatus("Redraw", new()
        {
            Definition = new()
            {
                icon = sprites["icon_redraw"].Sprite,
                color = new("FF0000")
            },
            Name = this.AnyLocalizations.Bind(["status", "redraw", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "redraw", "description"]).Localize
        });

        CustomPartsStatus = helper.Content.Statuses.RegisterStatus("CustomParts", new()
        {
            Definition = new()
            {
                icon = sprites["icon_customParts"].Sprite,
                color = new("777777")
            },
            Name = this.AnyLocalizations.Bind(["status", "customParts", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "customParts", "description"]).Localize
        });

        // animations
        RegisterAnimation("classy");
        RegisterAnimation("maniacal");
        RegisterAnimation("squint");
        RegisterAnimation("surprise");
        RegisterAnimation("sheepish");
        RegisterAnimation("excited");
        RegisterSingleFrameAnimation("proud");
        RegisterSingleFrameAnimation("whatisthat");
        RegisterSingleFrameAnimation("unhappy");
        RegisterSingleFrameAnimation("hot_chocolate");
        Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = PhilipDeck.Deck,
            LoopTag = "laugh",

            Frames = new[] { sprites["philip_laugh_0"].Sprite, sprites["philip_laugh_1"].Sprite, sprites["philip_laugh_0"].Sprite, sprites["philip_laugh_1"].Sprite, }
        });

        // register cards
        var cardTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => String.Equals(t.Namespace, "clay.PhilipTheMechanic.Cards", StringComparison.Ordinal))
            .ToList();
        foreach(var cardType in cardTypes)
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
            Art = ModEntry.Instance.sprites["card_Nanobots_2"].Sprite
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
            Art = ModEntry.Instance.sprites["card_Uranium_Rounds"].Sprite
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
            Art = ModEntry.Instance.sprites["card_Impromptu_Blast_Shield"].Sprite
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
            Art = ModEntry.Instance.sprites["card_Uh_Oh"].Sprite
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
        Api.RegisterOnRedrawHook(new EndlessToolboxHook(), 0);
        Api.RegisterAllowRedrawHook(new HotChocolateHook());
        Api.RegisterRedrawCostHook(new HotChocolateHook(), 0);
        Api.RegisterAllowRedrawHook(new ScrapMagnetHook());
        Api.RegisterRedrawCostHook(new ScrapMagnetHook(), 0);

        // TODO: register dialogue
        ShoutRegisterer.RegisterShout
        (
            StandardShoutHooks.Relevance7.EnemyHasWeakness, 
            PhilipDeck.Deck, 
            AnyLocalizations.Bind(["dialogue", "shout", "EnemyHasWeakness"]).Localize("en")!, 
            looptag: "classy" // optional
        );

    }

    private void RegisterSprite(IPluginPackage<IModManifest> package, string name)
    {
        sprites[name] = Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/{name}.png"));
    }
    private ICharacterAnimationEntry RegisterAnimation(string looptag)
    {
        return Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = PhilipDeck.Deck,
            LoopTag = looptag,

            Frames = new[] { sprites[$"philip_{looptag}_0"].Sprite, sprites[$"philip_{looptag}_1"].Sprite, sprites[$"philip_{looptag}_0"].Sprite, sprites[$"philip_{looptag}_3"].Sprite, }
        });
    }
    private void RegisterSingleFrameAnimation(string looptag)
    {
        Helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = PhilipDeck.Deck,
            LoopTag = looptag,

            Frames = new[] { sprites[$"philip_{looptag}"].Sprite }
        });
    }
}
