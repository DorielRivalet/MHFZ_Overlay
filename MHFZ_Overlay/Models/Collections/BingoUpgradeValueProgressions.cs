// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The bingo upgrade value progressions.
/// </summary>
public static class BingoUpgradeValueProgressions
{
    public static ReadOnlyDictionary<BingoUpgradeType, BingoUpgradeValueProgression> ValueProgressions { get; } = new (new Dictionary<BingoUpgradeType, BingoUpgradeValueProgression>
        {
            {
                BingoUpgradeType.BaseScoreMultiplier,
                new BingoUpgradeValueProgression { InitialValue = 1, ValueIncreasePerLevel = 0.1 }
            },

            {
                BingoUpgradeType.BaseScoreFlatIncrease,
                new BingoUpgradeValueProgression { InitialValue = 0, ValueIncreasePerLevel = 2 }
            },

            {
                BingoUpgradeType.CartsScore,
                new BingoUpgradeValueProgression { InitialValue = 0, ValueIncreasePerLevel = 3 }
            },

            {
                BingoUpgradeType.BonusScore,
                new BingoUpgradeValueProgression { InitialValue = 0, ValueIncreasePerLevel = 10 }
            },

            {
                BingoUpgradeType.MiddleSquareMultiplier,
                new BingoUpgradeValueProgression { InitialValue = 1, ValueIncreasePerLevel = 0.05 }
            },

            {
                BingoUpgradeType.WeaponMultiplier,
                new BingoUpgradeValueProgression { InitialValue = 1, ValueIncreasePerLevel = 0.02 }
            },
        });
}
