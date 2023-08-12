// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;
using NLog;

public sealed class BingoService
{
    public static BingoService GetInstance()
    {
        if (instance == null)
        {
            LoggerInstance.Debug("Singleton not found, creating instance.");
            instance = new BingoService();
        }

        LoggerInstance.Debug("Singleton found, returning instance.");
        LoggerInstance.Trace(new StackTrace().ToString());
        return instance;
    }

    // TODO database
    public double BaseScoreMultiplier { get; set; }

    public double BaseScoreFlatIncrease { get; set; }

    public double WeaponMultiplier { get; set; }

    public double MiddleSquareMultiplier { get; set; }

    public double BonusScore { get; set; }

    public double CartsScore { get; set; }

    public int PlayerBingoPoints { get; set; }

    public void ApplyUpgrade(BingoUpgrade upgrade)
    {
        if (upgrade.CurrentLevel < upgrade.MaxLevel)
        {
            // Calculate the cost for the next level based on cost progression
            var costProgression = BingoUpgradeCostProgressions.CostProgressions[upgrade.Type];
            int nextLevel = upgrade.CurrentLevel + 1;
            int nextCost = costProgression.CalculateUpgradeCost(nextLevel);

            if (PlayerBingoPoints >= nextCost)
            {
                PlayerBingoPoints -= nextCost;
                upgrade.CurrentLevel++;
                ApplyUpgradeValue(upgrade);
            }
        }
    }

    /// <summary>
    /// Calculates the bingo points obtained on the square.
    /// </summary>
    /// <param name="baseScore"></param>
    /// <returns>The bingo points obtained.</returns>
    public int CalculateBingoPoints(int baseScore, Difficulty difficulty, int carts)
    {
        // bingo points = ((((base score + base score flat increase) * base score multiplier) + carts score) * weapon multiplier * middle square multiplier * extreme difficulty multiplier) + bonus score
        var extremeDifficultyMultiplier = difficulty == Difficulty.Extreme ? 2 : 1;
        var cartsPenalty = 1.0;
        switch (carts)
        {
            case 1:
                cartsPenalty = 2.0;
                break;
            case >= 2:
                cartsPenalty = 3.0;
                break;
        }
        var cartsScore = CartsScore / cartsPenalty;
        double score =
            (
                (
                    (
                        (baseScore + BaseScoreFlatIncrease)
                        * BaseScoreMultiplier
                    ) + cartsScore
                ) * WeaponMultiplier * MiddleSquareMultiplier * extremeDifficultyMultiplier
            ) + BonusScore;
        return (int)Math.Ceiling(score);
    }

    public void ApplyUpgradeValue(BingoUpgrade upgrade)
    {
        if (BingoUpgradeValueProgressions.ValueProgressions.TryGetValue(upgrade.Type, out var valueProgression))
        {
            // Apply the upgrade's effects on the bingo game using the value progression
            double valueIncrease = valueProgression.CalculateValue(upgrade.CurrentLevel);

            switch (upgrade.Type)
            {
                case BingoUpgradeType.BaseScoreFlatIncrease:
                    BaseScoreFlatIncrease += valueIncrease;
                    break;
                case BingoUpgradeType.BaseScoreMultiplier:
                    BaseScoreMultiplier += valueIncrease;
                    break;
                case BingoUpgradeType.BonusScore:
                    BonusScore += valueIncrease;
                    break;
                case BingoUpgradeType.MiddleSquareMultiplier:
                    MiddleSquareMultiplier += valueIncrease;
                    break;
                case BingoUpgradeType.WeaponMultiplier:
                    WeaponMultiplier += valueIncrease;
                    break;
                case BingoUpgradeType.CartsScore:
                    CartsScore += valueIncrease;
                    break;
            }
        }
        else
        {
            LoggerInstance.Error(CultureInfo.InvariantCulture, "Could not find value progression for {0}", upgrade.Type);
        }
    }

    private BingoService() => LoggerInstance.Info($"Service initialized");
    private static readonly Logger LoggerInstance = LogManager.GetCurrentClassLogger();
    private static readonly DatabaseService DatabaseServiceInsance = DatabaseService.GetInstance();
    private static BingoService? instance;
}
