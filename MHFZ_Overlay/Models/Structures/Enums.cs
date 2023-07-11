// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Structures;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GachaCardTypes : uint
{
    Monster,
    Endemic_Life,
    Achievement,
    Item,
    Gear,
    Character,
    Location,
    Special,
}

public enum RankTypes : uint
{
    Low_Rank,
    High_Rank,
    G_Rank,
    Zenith_Rank,
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
    True,
    Effective,
}
