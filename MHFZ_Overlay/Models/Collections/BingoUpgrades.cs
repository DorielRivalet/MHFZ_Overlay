// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MHFZ_Overlay.Models.Structures;

public static class BingoUpgrades
{
    public static ReadOnlyDictionary<int, BingoUpgrade> IDBingoUpgrade { get; } = new(new Dictionary<int, BingoUpgrade>
    {
        {
            0, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Gives a flat increase to the monster base score. Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Base Score Increase",
                Type = BingoUpgradeType.BaseScoreFlatIncrease,
            }
        },
        {
            1, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Multiplies the monster base score by a set multiplier. Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Base Score Multiplier",
                Type = BingoUpgradeType.BaseScoreMultiplier,
            }
        },
        {
            2, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier if the weapon type used in the quest corresponds to the weapon shown on the grid. Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Weapon Type Multiplier",
                Type = BingoUpgradeType.WeaponMultiplier,
            }
        },
        {
            3, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set amount depending on the amount of times you carted on the quest (e.g. if carts score is 30, then no carts means 30, 1 cart means 20, and 2 carts or more means 10 more points respectively). Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Carts Score",
                Type = BingoUpgradeType.CartsScore,
            }
        },
        {
            4, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Gives a flat final bonus score, unaffected by multipliers and is calculated last. Modifies bingo run final score.",
                MaxLevel = 10,
                Name = "Bonus Score",
                Type = BingoUpgradeType.BonusScore,
            }
        },
        {
            5, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier if the quest completed corresponds to the middle square of the bingo board. Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Middle Square Multiplier",
                Type = BingoUpgradeType.MiddleSquareMultiplier,
            }
        },
        {
            6, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the amount of carts allowed at the start of a bingo run. Faints from quest cancels are not counted, only completed quests are counted.",
                MaxLevel = 4,
                Name = "Extra Carts",
                Type = BingoUpgradeType.ExtraCarts,
            }
        },
        {
            7, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Reduces the starting cost of a bingo run by a set percentage.",
                MaxLevel = 10,
                Name = "Starting Cost Reduction",
                Type = BingoUpgradeType.StartingCostReduction,
            }
        },
        {
            8, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the chance of an extreme difficulty bingo board to have its middle square be rerolled at the start of a bingo run.",
                MaxLevel = 100,
                Name = "Middle Square Reroll Chance (Extreme Difficulty)",
                Type = BingoUpgradeType.MiddleSquareRerollChance,
            }
        },
        {
            9, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the amount of rerolls for Burning Freezing Elzelion appearing in a random cell at the start of a bingo run. Can only be activated if the bingo board difficulty is set to Extreme.",
                MaxLevel = 100,
                Name = "Burning Freezing Elzelion Rerolls (Extreme Difficulty)",
                Type = BingoUpgradeType.BurningFreezingElzelionRerolls,
            }
        },
        {
            10, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the chance for Burning Freezing Elzelion rerolls to succeed at the start of a bingo run. Can only be activated if the bingo board difficulty is set to Extreme.",
                MaxLevel = 10,
                Name = "Burning Freezing Elzelion Reroll Chance (Extreme Difficulty)",
                Type = BingoUpgradeType.BurningFreezingElzelionRerollChance,
            }
        },
        {
            11, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier according to the amount of obtained achievements. Achievements that are secret do not count. Modifies bingo run final score.",
                MaxLevel = 10,
                Name = "Achievements Multiplier",
                Type = BingoUpgradeType.AchievementMultiplier,
            }
        },
        {
            12, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending on the amount of secret achievements obtained. Modifies bingo run final score",
                MaxLevel = 5,
                Name = "Secret Achievements Multiplier",
                Type = BingoUpgradeType.SecretAchievementMultiplier,
            }
        },
        {
            13, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending on the amount of bingo runs completed. Modifies bingo run final score.",
                MaxLevel = 10,
                Name = "Bingo Completions Multiplier",
                Type = BingoUpgradeType.BingoCompletionsMultiplier,
            }
        },
        {
            14, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending if the square completed contains a zenith monster. Requires Gauntlet Boost to activate (1 Zenith Gauntlet item to use). Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Zenith Square Multiplier",
                Type = BingoUpgradeType.ZenithMultiplier,
            }
        },
        {
            15, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending if the square completed contains a conquest or shiten monster. Requires Gauntlet Boost to activate (1 Solstice Gauntlet item to use). Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Conquest/Shiten Square Multiplier",
                Type = BingoUpgradeType.SolsticeMultiplier,
            }
        },
        {
            16, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending if the square completed contains a musou monster. Requires Gauntlet Boost to activate (1 Musou Gauntlet item to use). Modifies bingo square score.",
                MaxLevel = 10,
                Name = "Musou Square Multiplier",
                Type = BingoUpgradeType.MusouMultiplier,
            }
        },
        {
            17, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending if the completed bingo run was done with a horizontal line. Modifies bingo run final score.",
                MaxLevel = 10,
                Name = "Horizontal Line Bingo Completion Multiplier",
                Type = BingoUpgradeType.HorizontalLineCompletionMultiplier,
            }
        },
        {
            18, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending if the completed bingo run was done with a vertical line. Modifies bingo run final score.",
                MaxLevel = 10,
                Name = "Vertical Line Bingo Completion Multiplier",
                Type = BingoUpgradeType.VerticalLineCompletionMultiplier,
            }
        },
        {
            19, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending if the completed bingo run was done with a diagonal line. Modifies bingo run final score.",
                MaxLevel = 10,
                Name = "Diagonal Line Bingo Completion Multiplier",
                Type = BingoUpgradeType.DiagonalLineCompletionMultiplier,
            }
        },
        {
            20, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the score by a set multiplier depending on the time it took to comple the bingo run, as shown in the timer close to the bingo board. Modifies bingo run final score.",
                MaxLevel = 10,
                Name = "Bingo Run Time Completion Multiplier",
                Type = BingoUpgradeType.RealTimeMultiplier,
            }
        },
        {
            21, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the chance of finding a page in a cell at board generation.",
                MaxLevel = 10,
                Name = "Page Finder Chance",
                Type = BingoUpgradeType.PageFinderChance,
            }
        },
        {
            22, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the chance of finding an ancient dragon part's scraps in a cell at board generation.",
                MaxLevel = 10,
                Name = "Scrap Finder Chance",
                Type = BingoUpgradeType.AncientDragonPartScrapChance,
            }
        },
        {
            23, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the rate for the compound interest of the currently stored bingo points. It compounds x times where x is the amount of bingo cells completed in a run. The compound interest is calculated at the end of a run, taking into account the points obtained in the run plus the currently stored points.",
                MaxLevel = 10,
                Name = "Bingo Points Compound Interest Rate",
                Type = BingoUpgradeType.BingoPointsCompoundInterestRate,
            }
        },
        {
            24, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Reduces the cost of upgrades.",
                MaxLevel = 10,
                Name = "Upgrade Cost Reduction",
                Type = BingoUpgradeType.BingoShopUpgradeReduction,
            }
        },
        {
            25, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the rate or speed at which the transcend meter fills.",
                MaxLevel = 10,
                Name = "Transcend Meter Fill Rate",
                Type = BingoUpgradeType.TranscendMeterFillRate,
            }
        },
        {
            26, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Reduces the cost of a true transcend. Lowers the transcend meter capacity, which might be a drawback when using normal transcend.",
                MaxLevel = 10,
                Name = "True Transcend Cost Reduction",
                Type = BingoUpgradeType.TrueTranscendCostReduction,
            }
        },
        {
            27, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Decreases the rate at which the transcend meter drains.",
                MaxLevel = 10,
                Name = "Transcend Meter Drain Reduction",
                Type = BingoUpgradeType.TranscendMeterDrainReduction,
            }
        },
        {
            28, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "Increases the grace period for obtaining the maximum time score.",
                MaxLevel = 10,
                Name = "Maximum Time Score Grace Period Increase",
                Type = BingoUpgradeType.MaxTimeScoreGracePeriodIncrease,
            }
        },
        {
            29, new BingoUpgrade
            {
                CurrentLevel = 1,
                Description = "You found a very old book, the shop owner says you can take it if you complete a bingo.",
                MaxLevel = 2,
                Name = "Book of Secrets Tome I",
                Type = BingoUpgradeType.BookOfSecretsTomeOne,
                IsUnlocked = true,
            }
        },
    });
}
