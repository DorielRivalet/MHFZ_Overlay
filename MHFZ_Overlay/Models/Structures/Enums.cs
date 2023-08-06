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
