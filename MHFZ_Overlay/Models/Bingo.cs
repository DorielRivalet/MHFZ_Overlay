// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using MHFZ_Overlay.Models.Structures;

// TODO: ORM
public sealed class Bingo
{
    /// <summary>
    /// Primary key autoincrement.
    /// </summary>
    public long BingoID { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// The difficulty of the bingo.
    /// </summary>
    public Difficulty Difficulty { get; set; }

    /// <summary>
    /// The list of run IDs related to the bingo run.
    /// </summary>
    public List<int> MonsterList { get; set; } = new List<int>();

    /// <summary>
    /// Unused. The weapon type used when bingo finished.
    /// </summary>
    public string WeaponType { get; set; } = string.Empty;

    /// <summary>
    /// The category of the bingo.
    /// </summary>
    public BingoGauntletCategory Category { get; set; }

    /// <summary>
    /// The total amount of frames elapsed (sum of RunID, which is the MonsterList, Quests frames elapsed).
    /// </summary>
    public long TotalFramesElapsed { get; set; } = long.MaxValue;

    /// <summary>
    /// The TotalFramesElapsed as a string format of HH:MM:SS.ff.
    /// </summary>
    public string TotalTimeElapsed { get; set; } = string.Empty;

    /// <summary>
    /// The bingo score at bingo end.
    /// </summary>
    public long Score { get; set; }
}
