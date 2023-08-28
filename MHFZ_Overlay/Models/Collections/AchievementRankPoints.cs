// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The base points depending on achievement rank.
/// </summary>
public static class AchievementRankPoints
{
    public static ReadOnlyDictionary<AchievementRank, int> RankPoints { get; } = new(new Dictionary<AchievementRank, int>
        {
            {
                AchievementRank.None, 0
            },
            {
                AchievementRank.Bronze, 5
            },
            {
                AchievementRank.Silver, 10
            },
            {
                AchievementRank.Gold, 15
            },
            {
                AchievementRank.Platinum, 30
            },
        });
}
