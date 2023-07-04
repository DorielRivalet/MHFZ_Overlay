// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay.Core.Class.Dictionary;
using MHFZ_Overlay.Core.Enum;
using System;
using System.Collections.Generic;

namespace MHFZ_Overlay.UI.Class;

//TODO: ORM
public class Bingo
{
    public long BingoID { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; } = string.Empty;
    public Difficulty Difficulty { get; set; }
    public List<int> MonsterList { get; set; } = new List<int>();
    public string WeaponType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public long TotalFramesElapsed { get; set; } = long.MaxValue;
    public string TotalTimeElapsed { get; set; } = string.Empty;
    public long Score { get; set; } = 0;
}
