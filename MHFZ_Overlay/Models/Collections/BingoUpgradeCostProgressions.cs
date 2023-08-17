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
    public static ReadOnlyDictionary<BingoUpgradeType, LevelProgressionLinear> LinearCostProgressions { get; } = new(new Dictionary<BingoUpgradeType, LevelProgressionLinear>
        {
            {
                BingoUpgradeType.MiddleSquareRerollChance,
                new LevelProgressionLinear { InitialValue = 1000, ValueIncreasePerLevel = 990 }
            },
        });

    public static ReadOnlyDictionary<BingoUpgradeType, LevelProgressionExponential> ExponentialCostProgressions { get; } = new (new Dictionary<BingoUpgradeType, LevelProgressionExponential>
        {
            {
                BingoUpgradeType.BaseScoreFlatIncrease,
                new LevelProgressionExponential { InitialValue = 10, ValueIncreaseFactor = 2 }
            },
            {
                BingoUpgradeType.BaseScoreMultiplier,
                new LevelProgressionExponential { InitialValue = 50, ValueIncreaseFactor = 1.8M }
            },
            {
                BingoUpgradeType.WeaponMultiplier,
                new LevelProgressionExponential { InitialValue = 500, ValueIncreaseFactor = 1.4M }
            },
            {
                BingoUpgradeType.CartsScore,
                new LevelProgressionExponential { InitialValue = 250, ValueIncreaseFactor = 1.6M }
            },
            {
                BingoUpgradeType.BonusScore,
                new LevelProgressionExponential { InitialValue = 1200, ValueIncreaseFactor = 1.2M }
            },
            {
                BingoUpgradeType.MiddleSquareMultiplier,
                new LevelProgressionExponential { InitialValue = 2500, ValueIncreaseFactor = 1.1M }
            },
            {
                BingoUpgradeType.ExtraCarts,
                new LevelProgressionExponential { InitialValue = 1000, ValueIncreaseFactor = 2 }
            },
            {
                BingoUpgradeType.StartingCostReduction,
                new LevelProgressionExponential { InitialValue = 800, ValueIncreaseFactor = 1.5M }
            },
            {
                BingoUpgradeType.BurningFreezingElzelionRerolls,
                new LevelProgressionExponential { InitialValue = 500, ValueIncreaseFactor = 1.4M }
            },
            {
                BingoUpgradeType.BurningFreezingElzelionRerollChance,
                new LevelProgressionExponential { InitialValue = 500, ValueIncreaseFactor = 1.4M }
            },
            {
                BingoUpgradeType.AchievementMultiplier,
                new LevelProgressionExponential { InitialValue = 500, ValueIncreaseFactor = 1.4M }
            },
            {
                BingoUpgradeType.SecretAchievementMultiplier,
                new LevelProgressionExponential { InitialValue = 500, ValueIncreaseFactor = 1.4M }
            },
        });
}
