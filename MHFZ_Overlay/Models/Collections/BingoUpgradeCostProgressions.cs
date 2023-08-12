// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The bingo upgrade cost progressions.
/// </summary>
public static class BingoUpgradeCostProgressions
{
    public static ReadOnlyDictionary<BingoUpgradeType, BingoUpgradeCostProgression> CostProgressions { get; } = new (new Dictionary<BingoUpgradeType, BingoUpgradeCostProgression>
        {
            {
                BingoUpgradeType.BaseScoreMultiplier,
                new BingoUpgradeCostProgression { InitialCost = 50, CostIncreaseFactor = 1.8 }
            },

            {
                BingoUpgradeType.BaseScoreFlatIncrease,
                new BingoUpgradeCostProgression { InitialCost = 10, CostIncreaseFactor = 2 }
            },

            {
                BingoUpgradeType.CartsScore,
                new BingoUpgradeCostProgression { InitialCost = 250, CostIncreaseFactor = 1.6 }
            },

            {
                BingoUpgradeType.BonusScore,
                new BingoUpgradeCostProgression { InitialCost = 1200, CostIncreaseFactor = 1.2 }
            },

            {
                BingoUpgradeType.MiddleSquareMultiplier,
                new BingoUpgradeCostProgression { InitialCost = 2500, CostIncreaseFactor = 1.1 }
            },

            {
                BingoUpgradeType.WeaponMultiplier,
                new BingoUpgradeCostProgression { InitialCost = 500, CostIncreaseFactor = 1.4 }
            },
        });
}
