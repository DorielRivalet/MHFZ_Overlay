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
    TimeAttack,
    FreestyleSecretTech,
    Freestyle,
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
