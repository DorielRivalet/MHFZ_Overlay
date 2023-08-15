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
                new LevelProgressionLinear { InitialValue = 1.1, ValueIncreasePerLevel = 0.1 }
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
                new LevelProgressionLinear { InitialValue = 1.05, ValueIncreasePerLevel = 0.05 }
            },
            {
                BingoUpgradeType.WeaponMultiplier,
                new LevelProgressionLinear { InitialValue = 1.02, ValueIncreasePerLevel = 0.02 }
            },
            {
                BingoUpgradeType.ExtraCarts,
                new LevelProgressionLinear { InitialValue = 0, ValueIncreasePerLevel = 1 }
            },
            {
                BingoUpgradeType.StartingCostReduction,
                new LevelProgressionLinear { InitialValue = 0.05, ValueIncreasePerLevel = 0.05 }
            },
            {
                BingoUpgradeType.MiddleSquareRerollChance,
                new LevelProgressionLinear { InitialValue = 0.0001, ValueIncreasePerLevel = 0.0001 }
            },
            {
                BingoUpgradeType.BurningFreezingElzelionRerolls,
                new LevelProgressionLinear { InitialValue = 1, ValueIncreasePerLevel = 1 }
            },
            {
                BingoUpgradeType.BurningFreezingElzelionRerollChance,
                new LevelProgressionLinear { InitialValue = 0.01, ValueIncreasePerLevel = 0.01 }
            },
            {
                BingoUpgradeType.AchievementMultiplier,
                new LevelProgressionLinear { InitialValue = 0.001, ValueIncreasePerLevel = 0.001 }
            },
            {
                BingoUpgradeType.SecretAchievementMultiplier,
                new LevelProgressionLinear { InitialValue = 1, ValueIncreasePerLevel = 1 }
            },
            {
                BingoUpgradeType.BingoCompletionsMultiplier,
                new LevelProgressionLinear { InitialValue = 1.1, ValueIncreasePerLevel = 0.1 }
            },
        });
}
