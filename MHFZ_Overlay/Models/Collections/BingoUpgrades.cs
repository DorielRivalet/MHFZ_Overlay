// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MHFZ_Overlay.Models.Structures;

public sealed class BingoUpgrades
{
    public static ReadOnlyDictionary<int, BingoUpgrade> IDBingoUpgrade { get; } = new(new Dictionary<int, BingoUpgrade>
    {
        {
            0, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Gives a flat increase to the monster base score.",
                MaxLevel = 10,
                Name = "Base Score Increase",
                Type = BingoUpgradeType.BaseScoreFlatIncrease,
            }
        },
        {
            1, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Multiplies the monster base score by a set multiplier.",
                MaxLevel = 10,
                Name = "Base Score Multiplier",
                Type = BingoUpgradeType.BaseScoreMultiplier,
            }
        },
        {
            2, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier if the weapon type used in the quest corresponds to the weapon shown on the grid.",
                MaxLevel = 10,
                Name = "Weapon Type Multiplier",
                Type = BingoUpgradeType.WeaponMultiplier,
            }
        },
        {
            3, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set amount depending on the amount of times you carted on the quest (e.g. if carts score is 30, then no carts means 30, 1 cart means 20, and 2 carts or more means 10 more points respectively)",
                MaxLevel = 10,
                Name = "Carts Score",
                Type = BingoUpgradeType.CartsScore,
            }
        },
        {
            4, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Gives a flat final bonus score, unaffected by multipliers and is calculated last.",
                MaxLevel = 10,
                Name = "Bonus Score",
                Type = BingoUpgradeType.BonusScore,
            }
        },
        {
            5, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier if the quest completed corresponds to the middle square of the bingo board.",
                MaxLevel = 10,
                Name = "Middle Square Multiplier",
                Type = BingoUpgradeType.MiddleSquareMultiplier,
            }
        },
    });
}
