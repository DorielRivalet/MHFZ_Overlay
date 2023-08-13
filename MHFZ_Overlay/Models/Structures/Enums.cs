// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Structures;

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
    /// Flat increase to the base score before multiplying by BaseScoreMultiplier.
    /// </summary>
    BaseScoreFlatIncrease,

    /// <summary>
    /// Multiplies the base score.
    /// </summary>
    BaseScoreMultiplier,

    /// <summary>
    /// The multiplier for using a certain weapon type.
    /// </summary>
    WeaponMultiplier,

    /// <summary>
    /// Flat increase with score varying by the amount of carts.
    /// </summary>
    CartsScore,

    /// <summary>
    /// Flat increase to the score, as final calculation, unaffected by any multiplier.
    /// </summary>
    BonusScore,

    /// <summary>
    /// The multiplier for the middle square being completed.
    /// </summary>
    MiddleSquareMultiplier,
}

public enum FrontierWeaponTypes
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
