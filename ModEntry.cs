using clay.PhilipTheMechanic.Cards;
using clay.PhilipTheMechanic.Controllers;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/* In the Cobalt Core modding community it is common for namespaces to be <Author>.<ModName>
 * This is helpful to know at a glance what mod we're looking at, and who made it */
namespace clay.PhilipTheMechanic;

/* ModEntry is the base for our mod. Others like to name it Manifest, and some like to name it <ModName>
 * Notice the ': SimpleMod'. This means ModEntry is a subclass (child) of the superclass SimpleMod (parent). This is help us use Nickel's functions more easily! */
public sealed class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal readonly IPhilipAPI Api = new ApiImplementation();

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
        RegisterSprite(package, "icon_screw");
        RegisterSprite(package, "icon_equal");
        RegisterSprite(package, "icon_redraw");
        RegisterSprite(package, "icon_customParts");
        RegisterSprite(package, "icon_no_action");
        RegisterSprite(package, "icon_card_is_centered");
        RegisterSprite(package, "icon_card_is_not_centered");
        RegisterSprite(package, "icon_card_is_centered_disabled");
        RegisterSprite(package, "icon_card_is_not_centered_disabled");
        RegisterSprite(package, "icon_flimsy");
    
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
        RegisterSprite(package, "icon_sticker_missile");
        RegisterSprite(package, "icon_sticker_hermes");
        RegisterSprite(package, "icon_sticker_stun");
        RegisterSprite(package, "icon_sticker_recycle");
        RegisterSprite(package, "icon_sticker_redraw");
        RegisterSprite(package, "icon_sticker_retain");
        RegisterSprite(package, "icon_sticker_no_action");
        RegisterSprite(package, "icon_sticker_energyLessNextTurn");
        RegisterSprite(package, "icon_sticker_card_unplayable");
    
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

            AccessTools.DeclaredMethod(cardType, nameof(IRegisterableCard.Register))?.Invoke(null, [helper]);
        }

        // one for every artifact
        //AccessTools.DeclaredMethod(HotChocolate, nameof(IDemoArtifact.Register))?.Invoke(null, new() { helper });
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
