// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Structures;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GachaCardTypes : uint
{
    Monster,
    EndemicLife,
    Achievement,
    Item,
    Gear,
    Character,
    Location,
    Special,
}

public enum RankTypes : uint
{
    LowRank,
    HighRank,
    GRank,
    ZenithRank,
}

public enum AchievementRank
{
    None,
    Bronze,
    Silver,
    Gold,
    Platinum,
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
}

public enum Difficulty
{
    Unknown,
    Easy,
    Medium,
    Hard,
    Extreme,
}

public enum MonsterHPMode
{
    /// <summary>
    /// The monster defense rate is not taken into account
    /// </summary>
    True,

    /// <summary>
    /// The monster defense rate is taken into account
    /// </summary>
    Effective,
}

public enum GetterMode
{
    /// <summary>
    /// Gets a single object/primitive
    /// </summary>
    One,

    /// <summary>
    /// Gets the count of the object
    /// </summary>
    Count,

    /// <summary>
    /// Gets the sum of the object
    /// </summary>
    Sum,

    /// <summary>
    /// Gets the mean of the object
    /// </summary>
    Average,

    /// <summary>
    /// Gets the median of the object
    /// </summary>
    Median,

    /// <summary>
    /// Gets the mode of the object
    /// </summary>
    Mode,

    /// <summary>
    /// Gets objects/values from the object according to certain requirements
    /// </summary>
    Filtered,

    /// <summary>
    /// Gets the last entry of the object
    /// </summary>
    Last,

    /// <summary>
    /// Gets the first entry of the object
    /// </summary>
    First,

    /// <summary>
    /// Gets the middle entry or entries of the object
    /// </summary>
    Middle,

    /// <summary>
    /// Gets all of the object(s)
    /// </summary>
    All,
}

public enum SetterMode
{
    /// <summary>
    /// Inserts into the object
    /// </summary>
    Insert,

    /// <summary>
    /// Update the entry, if it doesn't exist then insert the entry into the object
    /// </summary>
    Upsert,

    /// <summary>
    /// Update an entry
    /// </summary>
    Update,

    /// <summary>
    /// Delete the entry/object
    /// </summary>
    Delete,

    /// <summary>
    /// Delete all of the object
    /// </summary>
    DeleteAll,

    /// <summary>
    /// Clear the object or entry
    /// </summary>
    Clear,

    /// <summary>
    /// Clear all of the object
    /// </summary>
    ClearAll,
}

public enum ChallengeState
{
    /// <summary>
    /// The challenge is available for start
    /// </summary>
    Idle,

    /// <summary>
    /// The challenge is currently in progress, other challenges cannot be running and must be idle.
    /// </summary>
    Running,
}

/// <summary>
/// The category of the bingo or gauntlet challenges.
/// </summary>
public enum BingoGauntletCategory
{
    Unknown,
    GreatSword,
    LongSword,
    DualSwords,
    SwordAndShield,
    Hammer,
    HuntingHorn,
    Lance,
    Gunlance,
    LightBowgun,
    HeavyBowgun,
    Bow,
    Tonfa,
    SwitchAxeF,
    MagnetSpike,

    /// <summary>
    /// If you used more than 1 weapon type in any quests during a bingo/gauntlet.
    /// </summary>
    Multiple,

    /// <summary>
    /// If the party size was greater than 1 in any quests during a bingo/gauntlet. Overrides Multiple.
    /// </summary>
    Multiplayer,
}

/// <summary>
/// The type of bingo upgrade.
/// </summary>
public enum BingoUpgradeType
{
    /// <summary>
    /// The flat increase to the base score before multiplying by BaseScoreMultiplier.
    /// </summary>
    BaseScoreFlatIncrease,

    /// <summary>
    /// The multiplier for the base score.
    /// </summary>
    BaseScoreMultiplier,

    /// <summary>
    /// The multiplier for using a certain weapon type.
    /// </summary>
    WeaponMultiplier,

    /// <summary>
    /// The score varying by the amount of carts.
    /// </summary>
    CartsScore,

    /// <summary>
    /// The flat increase to the score, as final calculation, unaffected by any multiplier.
    /// </summary>
    BonusScore,

    /// <summary>
    /// The multiplier for the middle square being completed.
    /// </summary>
    MiddleSquareMultiplier,

    /// <summary>
    /// The amount of extra carts when starting a bingo run.
    /// </summary>
    ExtraCarts,

    /// <summary>
    /// The reduction for the price of starting a bingo depending on difficulty.
    /// </summary>
    StartingCostReduction,

    /// <summary>
    /// The chance for the middle square in an extreme difficulty bingo to be rerolled.
    /// </summary>
    MiddleSquareRerollChance,

    /// <summary>
    /// The amount of times an extreme difficulty bingo board can be rerolled so that Burning Freezing Elzelion can show on a random cell, depending on BurningFreezingElzelionRerollChance.
    /// </summary>
    BurningFreezingElzelionRerolls,

    /// <summary>
    /// The chance for a Burning Freezing Elzelion to show in a random cell if rerolled.
    /// </summary>
    BurningFreezingElzelionRerollChance,

    /// <summary>
    /// The multiplier with value varying by the amount of obtained achievements, except secret achievements.
    /// </summary>
    AchievementMultiplier,

    /// <summary>
    /// The score varying by the amount of obtained secret achievements.
    /// </summary>
    SecretAchievementMultiplier,

    /// <summary>
    /// The multiplier with value varying by the amount of bingo runs completed (multiplies the bingo run completions by this multiplier then does log 2).
    /// </summary>
    BingoCompletionsMultiplier,

    /// <summary>
    /// The multiplier for the square completed containing a zenith monster.
    /// </summary>
    ZenithMultiplier,

    /// <summary>
    /// The multiplier for the square completed containing a conquest or shiten monster.
    /// </summary>
    SolsticeMultiplier,

    /// <summary>
    /// The multiplier for the square completed containing a musou monster.
    /// </summary>
    MusouMultiplier,

    /// <summary>
    /// The multiplier for the bingo run being completed on an horizontal line.
    /// </summary>
    HorizontalLineCompletionMultiplier,

    /// <summary>
    /// The multiplier for the bingo run being completed on a vertical line.
    /// </summary>
    VerticalLineCompletionMultiplier,

    /// <summary>
    /// The multiplier for the bingo run being completed on a diagonal line.
    /// </summary>
    DiagonalLineCompletionMultiplier,

    /// <summary>
    /// The multiplier for the time it took to complete the bingo from start button press to the moment the last square is completed on a line. Affected by certain multipliers, calculated right before BonusScore.
    /// </summary>
    RealTimeMultiplier,

    /// <summary>
    /// The chance of finding a page in a cell at board generation.
    /// </summary>
    PageFinderChance,

    /// <summary>
    /// The chance of finding an ancient dragon part's scraps in a cell at board generation.
    /// </summary>
    AncientDragonPartScrapChance,

    /// <summary>
    /// The rate for the compound interest of the bingo points. It compounds x times where x is the amount of bingo cells completed in a run.
    /// </summary>
    BingoPointsCompoundInterestRate,

    /// <summary>
    /// The cost reduction for the bingo shop upgrades.
    /// </summary>
    BingoShopUpgradeReduction,

    /// <summary>
    /// The rate/speed at which the transcend meter fills.
    /// </summary>
    TranscendMeterFillRate,

    /// <summary>
    /// The cost reduction of a true transcend (essentially lowers the meter capacity, which might be a drawback for using normal transcend).
    /// </summary>
    TrueTranscendCostReduction,

    /// <summary>
    /// Decreases the rate at which the transcend meter drains.
    /// </summary>
    TranscendMeterDrainReduction,

    /// <summary>
    /// Increases the grace period for obtaining the maximum time score.
    /// </summary>
    MaxTimeScoreGracePeriodIncrease,

    /// <summary>
    /// Unlocks the book of secrets tab.
    /// </summary>
    BookOfSecretsTomeOne,
}

public enum FrontierWeaponType
{
    GreatSword,
    HeavyBowgun,
    Hammer,
    Lance,
    SwordAndShield,
    LightBowgun,
    DualSwords,
    LongSword,
    HuntingHorn,
    Gunlance,
    Bow,
    Tonfa,
    SwitchAxeF,
    MagnetSpike,
}

public enum BingoLineColorOption
{
    /// <summary>
    /// The board will mark blue the run that gives the most points.
    /// </summary>
    Hardest,

    /// <summary>
    /// The board will mark blue the run that gives the least points.
    /// </summary>
    Easiest,
}

public enum BingoSquareMonsterType
{
    /// <summary>
    /// The default monster type.
    /// </summary>
    Default,

    /// <summary>
    /// The monster is a zenith.
    /// </summary>
    Zenith,

    /// <summary>
    /// The monster is a conquest or shiten monster.
    /// </summary>
    Solstice,

    /// <summary>
    /// The monster is a musou.
    /// </summary>
    Musou,
}

public enum BingoLineCompletionType
{
    /// <summary>
    /// The line is not known.
    /// </summary>
    Unknown,

    /// <summary>
    /// The bingo run was completed with a horizontal line.
    /// </summary>
    Horizontal,

    /// <summary>
    /// The bingo run was completed with a vertical line.
    /// </summary>
    Vertical,

    /// <summary>
    /// The bingo run was completed with a diagonal line.
    /// </summary>
    Diagonal,
}

// TODO enums for settings
public enum TimerMode
{
    TimeLeft,
    Elapsed,
}

public enum TimerFormat
{
    MinutesSeconds,
    MinutesSecondsMilliseconds,
    HoursMinutesSeconds,
}

public enum OverlayMode
{
    Unknown,
    Standard,
    Configuring,
    ClosedGame,
    Launcher,
    NoGame,
    MainMenu,
    WorldSelect,
    Speedrun,
    Zen,
}

public enum ConfigurationPreset
{
    None,
    Speedrun,
    Zen,
    HPOnly,
    All,
}

public enum FrontierSharpness
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    White,
    Purple,
    Cyan,
}

public enum FrontierMonsterType
{
    /// <summary>
    /// ???, Unclassified, Unknown, etc.
    /// </summary>
    Other,

    ElderDragon,
    Carapaceon,
    FlyingWyvern,
    BruteWyvern,
    PiscineWyvern,
    BirdWyvern,
    FangedBeast,
    Leviathan,
    FangedWyvern,
}

/// <summary>
/// Weapon and armor.
/// </summary>
public enum EquipmentSlot
{
    None,
    One,
    Two,
    Three
}

/// <summary>
/// <see href="https://github.com/var-username/Monster-Hunter-Frontier-Patterns/"/>
/// </summary>
public enum FrontierEquipmentType
{
    Legs,
    Unk1,
    Head,
    Chest,
    Arms,
    Waist,
    Melee,
    Ranged,
}

/// <summary>
/// <see href="https://github.com/var-username/Monster-Hunter-Frontier-Patterns/"/>
/// </summary>
public enum FrontierItemIcon : uint
{
    SMOKE = 0x00,           // Ex: Farcaster, Smoke Bomb
    ORB = 0x01,             // Ex: Paintball, Flash Bomb
    BOMB = 0x02,            // Ex: Small Barrel Bomb
    MEAT = 0x03,            // Ex: Raw Meat, Burnt Steak
    FISH_BAIT = 0x04,       // Ex: Cricket, Worm, Tuna Bait
    FISH = 0x05,            // Ex: Gormet Fish
    BOX = 0x06,             // Ex: Trap Kit
    WHETSTONE = 0x07,       // Ex: Whetstone
    DUNG = 0x08,            // Ex: Dung, Dung Bomb
    MONSTER = 0x09,         // Ex: Bullfango Head
    BONES = 0x0A,           // Ex: Sm Monster Bone
    BINOCULARS = 0x0B,      // Ex: Binoculars
    MUSHROOM = 0x0C,        // Ex: Blue Mushroom
    BUGNET = 0x0D,          // Ex: Bugnet
    PELT = 0x0E,            // Ex: Mosswine Hide, Bullfango Pelt
    LEAF = 0x0F,            // Ex: Herb
    PICKAXE = 0x10,         // Ex: Iron Pickaxe
    BARREL = 0x11,          // Ex: Small Barrel, Large Barrel
    SEED = 0x12,            // Ex: Paintberry, Power Seed, Scatternut
    BBQ_SPIT = 0x13,        // Ex: BBQ Spit
    INSECT = 0x14,          // Ex: Insect Husk, Thunderbug
    TRAP = 0x15,            // Ex: Pitfall Trap, Shock Trap
    NET = 0x16,             // Ex: Web, Net
    SCALE = 0x17,           // Ex: Broken Shell, Kut-Ku Scale
    DRINK = 0x18,           // Ex: Potion, Cold Drink, Demondrug
    EGG = 0x19,             // Ex: Monster Egg, Round Egg
    AMMO = 0x1A,            // Ex: Normal S LV1
    STONE = 0x1B,           // Ex: Stone, Iron Ore
    HUSK = 0x1C,            // Ex: Huskberry, Bone Husks
    MAP = 0x1D,             // Ex: Map
    FLUTE = 0x1E,           // Ex: Flute
    FANG = 0x1F,            // Ex: Wyvern Fang, Wyvern Claw
    GRID = 0x20,            // Ex: Honey
    QUESTION_MARK = 0x21,   // Ex: Garbage
    COIN = 0x22,            // Ex: N Medal, Hunting Medal
    SAC = 0x23,             // Ex: Catalyst, Screamer Sac, Might Pill
    BOOK = 0x24,            // Ex: Book of Combos
    TICKET = 0x25,          // Ex: Guild Ticket, Commendation, 30k Cheque
    KNIFE = 0x26,           // Ex: Throwing Knife
    MUSIC_SHEET = 0x29,     // Ex: HH Info
    JEWEL = 0x2B,           // Ex: Aquaglow Jewel, Attack 1 Deco
    HOUSE = 0x2C,           // Ex: House Expansion 1
    PlANT = 0x2D,           // Ex: Green Onion
    MOCHA = 0x2F,           // Ex: Mocha Green
    POT = 0x30,             // Ex: Mocha Pot
    BOOMERANG = 0x31,       // Ex: Boomerang
    COATING = 0x32,         // Ex: Power Coating, Poison Coating
    EMPTY_BOTTLE = 0x33,    // Ex: Empty Bottle
    CARAPACE = 0x34,        // Ex: Small LobsterShl, Hermitaur Shell
    /* Probably wrong
	COMPASS = 0x35,			// Datamined
	QUARTER_CIRCLE = 0x36,	// Datamined
	CROSS = 0x37,			// Datamined
	CROSSED_OUT = 0x3C,		// Datamined
	MYSTERY_WEAPON = 0x3D,	// Datamined
	GREATSWORD = 0x3E,		// Datamined
	HEAVY_BOWGUN = 0x3F,	// Datamined
	HAMMER = 0x40,			// Datamined
	LANCE = 0x41,			// Datamined
	SWORD_SHIELD = 0x42,	// Datamined
	LIGHT_BOWGUN = 0x43,	// Datamined
	DUAL_BLADES = 0x44,		// Datamined
	LONGSWORD = 0x45,		// Datamined
	HUNTING_HORN = 0x46, 	// Datamined
	GUNLANCE = 0x47,		// Datamined
	BOW = 0x48,				// Datamined
	LEGWEAR = 0x49,			// Datamined
	EQUIPED_MARKER = 0x4A,	// Datamined
	HEADWEAR = 0x4B,		// Datamined
	BODYWEAR = 0x4C,		// Datamined
	ARMWEAR = 0x4D,			// Datamined
	WAISTWEAR = 0x4E,		// Datamined
	*/
    SWORD_CRYSTAL = 0x4F,   // Ex: Bomb S.Crystal
    POTION = 0x50,          // Ex: Halk Pot
    FRUIT = 0x52,           // Ex: Shiriagari Fruit
    /* Definitely wrong
	SIGIL = 0x54,			// Datamined
	G_RANK_MARKER = 0x58,	// Datamined
	CAMERA = 0x5B,			// Datamined
	CORNER_ARROWS = 0x5C,	// Datamined
	TONFAS = 0x5D,			// Datamined
	TREASURE = 0x5E,		// Datamined
	PARTNYA_SWORD = 0x60,	// Datamined
	PARTNYA_CLUB = 0x61,	// Datamined
	PARTNYA_HELM = 0x62,	// Datamined
	PARTNYA_CHEST = 0x63,	// Datamined
	PARTNYA_HEAD = 0x64,	// Datamined
	PARTNYA_CROWN = 0x65,	// Datamined
	PARTNYA_MARKER = 0x66,	// Datamined
	TOWER_MARKER = 0x67,	// Datamined
	*/
    TOWER_W_SIGIL = 0x56,   // Ex: (SnS) Beam Slash
    TOWER_A_SIGIL = 0x5B,   // Ex: Skill UP Sigil
    MANTLE = 0x5C,          // Ex: Narga Mantle
    ARMOR_SPHERE = 0x5D,	// Ex: Armor Sphere
};

/// <summary>
/// <see href="https://github.com/var-username/Monster-Hunter-Frontier-Patterns/"/>
/// </summary>
public enum FrontierElement
{
    None,
    Fire,
	Water,
	Thunder,
	Dragon,
	Ice,
	Flame,
	Light,
	ThunderPole,
	Tenshou,
	Okiko,
	BlackFlame,
	Kanade,
	Darkness,
	CrimsonDemon,
	Wind,
	Sound,
	BurningZero,
	EmperorsRoar,
}

/// <summary>
/// <see href="https://github.com/var-username/Monster-Hunter-Frontier-Patterns/"/>
/// </summary>
public enum FrontierStatusAilment
{
    None,
    Poison,
	Paralysis,
    Sleep,
    Blast,
}

/// <summary>
/// <see href="https://github.com/var-username/Monster-Hunter-Frontier-Patterns/"/>
/// </summary>
public enum FrontierMap : uint
{
    Siege_Fortress_Day = 1,
    Forest_and_Hills_Day = 2,
    Desert_Day = 3,
    Swamp_Day = 4,
    Volcano_Day = 5,
    Jungle_Day = 6,
    Castle_Schrade = 7,
    Crimson_Battleground = 8,
    Arena_with_Ledge_Day = 9,
    Arena_with_Pillar_Day = 10,
    Snowy_Mountains_Day = 11,
    Town_Siege_Day = 12,
    Tower_1 = 13,
    Tower_2 = 14,
    Tower_3 = 15,
    Forest_and_Hills_Night = 16,
    Desert_Night = 17,
    Swamp_Night = 18,
    Volcano_Night = 19,
    Jungle_Night = 20,
    Snowy_Mountains_Night = 21,
    Town_Siege_night = 22,
    Siege_Fortress_Night = 23,
    Arena_with_Ledge_Night = 24,
    Arena_with_Pillar_Night = 25,
    Great_Forest_Day = 26,
    Great_Forest_Night = 27,
    Volcano_2_Day = 28,
    Volcano_2_Night = 29,
    Jungle_Dream = 30,
    Gorge_Day = 31,
    Gorge_Night = 32,
    Battlefield_Day = 35,
    Top_of_Great_Forest = 44,
    Caravan_Balloon_Day = 45,
    Caravan_Balloon_Night = 46,
    Solitude_Isle_1 = 47,
    Solitude_Isle_2 = 48,
    Solitude_Isle_3 = 49,
    Highlands_Day = 50,
    Highlands_Night = 51,
    Tower_with_Nesthole = 52,
    Arena_with_Moat_Day = 53,
    Arena_with_Moat_Night = 54,
    Fortress_Day = 55,
    Fortress_Night = 56,
    Tidal_Island_Day = 57,
    Tidal_Island_Night = 58,
    Polar_Sea_Day = 60,
    Polar_Sea_Night = 61,
    Worlds_End = 62,
    Large_Airship = 63,
    Flower_Field_Day = 64,
    Flower_Field_Night = 65,
    Deep_Crater = 66,
    Bamboo_Forest_Day = 67,
    Bamboo_Forest_Night = 68,
    Battlefield_2_Day = 69,
    Unimplemented_map = 70,
    Ist_Dist_Tower_1 = 71,
    Ist_Dist_Tower_2 = 72,
    Tnd_Dist_Tower_1 = 73,
    Tnd_Dist_Tower_2 = 74,
    Urgent_Tower = 75,
    n3rd_Dist_Tower = 76,
    n3rd_Dist_Tower_2 = 77,
    n4th_Dist_Tower = 78,
    White_Lake_Day = 79,
    White_Lake_Night = 80,
    Solitude_Depths_Slay_1 = 81,
    Solitude_Depths_Slay_2 = 82,
    Solitude_Depths_Slay_3 = 83,
    Solitude_Depths_Slay_4 = 84,
    Solitude_Depths_Slay_5 = 85,
    Solitude_Depths_Support_1 = 86,
    Solitude_Depths_Support_2 = 87,
    Solitude_Depths_Support_3 = 88,
    Solitude_Depths_Support_4 = 89,
    Solitude_Depths_Support_5 = 90,
    Cloud_Viewing_Fortress = 91,
    Painted_Falls_Day = 92,
    Painted_Falls_Night = 93,
    Sanctuary = 94,
    Hunters_Road = 95,
    Sacred_Pinnacle = 96,
    Historic_Site = 97
};

/// <summary>
/// <see href="https://github.com/var-username/Monster-Hunter-Frontier-Patterns/"/>
/// </summary>
public enum FrontierIconColor
{
    White = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Yellow = 4,
    Purple = 5,
    LightBlue = 6,
    Unk1 = 7,
    Pink = 8,
    Unk2 = 9,
    Gray = 10,
}

/// <summary>
/// <see href="https://github.com/var-username/Monster-Hunter-Frontier-Patterns/"/>
/// </summary>
public enum FrontierMonsterRank
{
    Unk1 = -1,
    LowRank = 0,
    HighRank = 1,
    Gou = 3,
    GRank = 7,
}

public enum DivaPrayerGemType
{
    None = 0,
    WindStorm = 1,
    Agility = 2,
    SeveringPower = 3,
    Elegance = 4,
    Earth = 5,
    Heaven = 6,
    Tempest = 7,
    CuttingEdge = 8,
    Striking = 9,
    RisingBullet = 10,
    StatusLength = 11,
    Abnormality = 12,
    Lethality = 13,
    HeavyThunder = 14,
    Unshakable = 15,
    Ringing = 16,
    Mobilisation = 17,
    Protection = 18,
    PowerfulStrikes = 19,
    Fireproof = 20,
    Waterproof = 21,
    Iceproof = 22,
    Dragonproof = 23,
    Thunderproof = 24,
    Immunity = 25,
}

public enum DivaPrayerGemColor
{
    None,
    Red,
    Yellow,
    Green,
    Blue,
}

/// <summary>
/// Creator, origin or purpose.
/// </summary>
public enum GamePatch
{
    Vanilla,
    Seph,
    Ezemania,
    Otyav1_1,
    Tenrou,
    Mezelounge,
    /// <summary>
    /// TODO
    /// </summary>
    Standard,
    FiveMusous,
}

/// <summary>
/// https://en.wikipedia.org/wiki/List_of_ISO_639_language_codes. Could also describe the origin rather than language.
/// </summary>
public enum GamePatchLanguage
{
    JA,
    EN,
    FR,
    ZH,
    ES,
    PT,
    RU,
    EL,
    DE,
}

public enum GamePatchFile
{
    dat,
    emd,
    dll,
    hddll,
}
