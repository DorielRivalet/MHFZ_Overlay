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
    public static ReadOnlyDictionary<BingoUpgradeType, LevelProgressionLinear> ValueProgressions { get; } = new (new Dictionary<BingoUpgradeType, LevelProgressionLinear>
        {
            {
                BingoUpgradeType.BaseScoreMultiplier,
                new LevelProgressionLinear { InitialValue = 1.1M, ValueIncreasePerLevel = 0.1M }
            },
            {
                BingoUpgradeType.BaseScoreFlatIncrease,
                new LevelProgressionLinear { InitialValue = 2, ValueIncreasePerLevel = 2 }
            },
            {
                BingoUpgradeType.CartsScore,
                new LevelProgressionLinear { InitialValue = 3, ValueIncreasePerLevel = 3 }
            },
            {
                BingoUpgradeType.BonusScore,
                new LevelProgressionLinear { InitialValue = 10, ValueIncreasePerLevel = 10 }
            },
            {
                BingoUpgradeType.MiddleSquareMultiplier,
                new LevelProgressionLinear { InitialValue = 1.05M, ValueIncreasePerLevel = 0.05M }
            },
            {
                BingoUpgradeType.WeaponMultiplier,
                new LevelProgressionLinear { InitialValue = 1.02M, ValueIncreasePerLevel = 0.02M }
            },
            {
                BingoUpgradeType.ExtraCarts,
                new LevelProgressionLinear { InitialValue = 0, ValueIncreasePerLevel = 1 }
            },
            {
                BingoUpgradeType.StartingCostReduction,
                new LevelProgressionLinear { InitialValue = 0.05M, ValueIncreasePerLevel = 0.05M }
            },
            {
                BingoUpgradeType.MiddleSquareRerollChance,
                new LevelProgressionLinear { InitialValue = 0.0001M, ValueIncreasePerLevel = 0.0001M }
            },
            {
                BingoUpgradeType.BurningFreezingElzelionRerolls,
                new LevelProgressionLinear { InitialValue = 1, ValueIncreasePerLevel = 1 }
            },
            {
                BingoUpgradeType.BurningFreezingElzelionRerollChance,
                new LevelProgressionLinear { InitialValue = 0.01M, ValueIncreasePerLevel = 0.01M }
            },
            {
                BingoUpgradeType.AchievementMultiplier,
                new LevelProgressionLinear { InitialValue = 0.001M, ValueIncreasePerLevel = 0.001M }
            },
            {
                BingoUpgradeType.SecretAchievementMultiplier,
                new LevelProgressionLinear { InitialValue = 1, ValueIncreasePerLevel = 1 }
            },
            {
                BingoUpgradeType.BingoCompletionsMultiplier,
                new LevelProgressionLinear { InitialValue = 1.1M, ValueIncreasePerLevel = 0.1M }
            },
        });
}
