// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
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

    /// <inheritdoc cref="BingoUpgradeType.BaseScoreMultiplier"/>
    public double BaseScoreMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.BaseScoreFlatIncrease"/>
    public double BaseScoreFlatIncrease { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.WeaponMultiplier"/>
    public double WeaponMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.MiddleSquareMultiplier"/>
    public double MiddleSquareMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.BonusScore"/>
    public double BonusScore { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.CartsScore"/>
    public double CartsScore { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.AchievementMultiplier"/>
    public double AchievementMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.SecretAchievementFlatIncrease"/>
    public double SecretAchievementFlatIncrease { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.BingoCompletionsMultiplier"/>
    public double BingoCompletionsMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.ZenithMultiplier"/>
    public double ZenithMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.SolsticeMultiplier"/>
    public double SolsticeMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.MusouMultiplier"/>
    public double MusouMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.HorizontalLineCompletionMultiplier"/>
    public double HorizontalLineCompletionMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.VerticalLineCompletionMultiplier"/>
    public double VerticalLineCompletionMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.DiagonalLineCompletionMultiplier"/>
    public double DiagonalLineCompletionMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.RealTimeMultiplier"/>
    public double RealTimeMultiplier { get; set; }

    /// <summary>
    /// Gets the player bingo points. Used for relegation. The data flow is View <-> ViewModel <-> Service <-> Database
    /// </summary>
    /// <returns>The player bingo points.</returns>
    public long GetPlayerBingoPoints() => DatabaseServiceInstance.GetPlayerBingoPoints();

    /// <summary>
    /// Buys the weapon rerolls.
    /// </summary>
    /// <param name="cost"></param>
    /// <returns>false if could not buy weapon rerolls.</returns>
    public bool BuyWeaponReroll(long cost)
    {
        var currentPlayerBingoPoints = GetPlayerBingoPoints();

        if (currentPlayerBingoPoints < cost)
        {
            return false;
        }

        currentPlayerBingoPoints -= cost;
        DatabaseServiceInstance.SetPlayerBingoPoints(currentPlayerBingoPoints);
        return true;
    }

    /// <summary>
    /// Applies the bingo upgrade.
    /// </summary>
    /// <param name="upgrade"></param>
    /// <returns>false if the upgrade could not be applied.</returns>
    public bool ApplyUpgrade(BingoUpgrade upgrade)
    {
        if (upgrade.CurrentLevel >= upgrade.MaxLevel)
        {
            return false;
        }

        // Calculate the cost for the next level based on cost progression
        // TODO optimize
        if (upgrade.Type == BingoUpgradeType.MiddleSquareRerollChance)
        {
            var costProgression = BingoUpgradeCostProgressions.LinearCostProgressions[upgrade.Type];
            int nextLevel = upgrade.CurrentLevel + 1;
            int nextCost = (int)costProgression.CalculateLinearValueForLevel(nextLevel);
            var playerBingoPoints = GetPlayerBingoPoints();

            if (playerBingoPoints < nextCost)
            {
                return false;
            }

            playerBingoPoints -= nextCost;
            DatabaseServiceInstance.SetPlayerBingoPoints(playerBingoPoints);
            upgrade.CurrentLevel++;
            ApplyUpgradeValue(upgrade);
            return true;
        }
        else
        {
            var costProgression = BingoUpgradeCostProgressions.ExponentialCostProgressions[upgrade.Type];
            int nextLevel = upgrade.CurrentLevel + 1;
            int nextCost = costProgression.CalculateExponentialValueForLevel(nextLevel);
            var playerBingoPoints = GetPlayerBingoPoints();

            if (playerBingoPoints < nextCost)
            {
                return false;
            }

            playerBingoPoints -= nextCost;
            DatabaseServiceInstance.SetPlayerBingoPoints(playerBingoPoints);
            upgrade.CurrentLevel++;
            ApplyUpgradeValue(upgrade);
            return true;
        }
    }

    /// <summary>
    /// Calculates the total bingo points obtained in a bingo run.
    /// </summary>
    /// <returns>The final score.</returns>
    public int CalculateBingoRunTotalPoints()
    {
        // TODO
        var baseScore = 0;//
        var difficulty = Difficulty.Unknown;//
        var carts = 0;//
        var obtainedAchievements = 0;
        var obtainedSecretAchievements = 0;
        var bingoCompletions = 0;
        var isMiddleSquare = false;
        var weaponBonusActive = false;

        var monsterType = BingoSquareMonsterType.Default;
        var monsterTypeMultiplier = 1.0;
        switch (monsterType)
        {
            case BingoSquareMonsterType.Zenith:
                monsterTypeMultiplier = ZenithMultiplier;
                break;
            case BingoSquareMonsterType.Solstice:
                monsterTypeMultiplier = SolsticeMultiplier;
                break;
            case BingoSquareMonsterType.Musou:
                monsterTypeMultiplier = MusouMultiplier;
                break;
        }

        var bingoLineType = BingoLineCompletionType.Unknown;
        var bingoLineTypeMultiplier = 1.0;
        switch (bingoLineType)
        {
            case BingoLineCompletionType.Diagonal:
                bingoLineTypeMultiplier = DiagonalLineCompletionMultiplier;
                break;
            case BingoLineCompletionType.Horizontal:
                bingoLineTypeMultiplier = HorizontalLineCompletionMultiplier;
                break;
            case BingoLineCompletionType.Vertical:
                bingoLineTypeMultiplier = VerticalLineCompletionMultiplier;
                break;
        }

        // TODO this has to change depending on the square state.
        // The below function would be called here and at board generation to show the max possible score on the squares.
        var squareBingoPoints = CalculateBingoSquarePoints(baseScore, carts, monsterTypeMultiplier, isMiddleSquare, weaponBonusActive);
        var squares = 5;
        var totalSquaresBingoPoints = squares * squareBingoPoints;

        var extremeDifficultyMultiplier = difficulty == Difficulty.Extreme ? 2 : 1;
        var obtainedAchievementsMultiplier = 1 + (AchievementMultiplier * obtainedAchievements);
        var obtainedSecretAchievementsScore = SecretAchievementFlatIncrease * obtainedSecretAchievements;

        // TODO
        var bingoCompletionsLogMultiplier = Math.Log2(1 + (bingoCompletions * BingoCompletionsMultiplier));

        // TODO
        var elapsedRealTimeInSeconds = 0;

        // TODO
        var realTimeScore = RealTimeMultiplier * extremeDifficultyMultiplier * 1;
        var maxRealTimeScore = 1000;
        int maxRealTimeScoreSecondsLimit = (int)TimeSpan.FromMinutes(10 * extremeDifficultyMultiplier).TotalSeconds;

        if (elapsedRealTimeInSeconds <= maxRealTimeScoreSecondsLimit)
        {
            realTimeScore = maxRealTimeScore;
        }
        else
        {
            realTimeScore = CalculateRealTimeScore(elapsedRealTimeInSeconds, realTimeScore, maxRealTimeScore, extremeDifficultyMultiplier);
        }

        // TODO
        // bingo ponts for final score = (total squares bingo points * extreme difficulty multiplier) +  + bonus score
        var finalScore = (totalSquaresBingoPoints * extremeDifficultyMultiplier) +
        (realTimeScore * bingoCompletionsLogMultiplier * obtainedAchievementsMultiplier) +
        obtainedSecretAchievementsScore +
        BonusScore;
        return (int)Math.Ceiling(finalScore);
    }

    public float CalculateRealTimeScore(int elapsedRealTimeInSeconds, double realTimeScore, int maxRealTimeScore, int extremeDifficultyMultiplier)
    {
        var maxRealTimeScoreLastSecond = TimeSpan.FromMinutes(30 * extremeDifficultyMultiplier).TotalSeconds;
        var fastDecreaseFromMaxScoreLastSecond = TimeSpan.FromMinutes(30 * extremeDifficultyMultiplier).TotalSeconds;
        var slowDecreaseFromHalvedScoreLastSecond = TimeSpan.FromMinutes(30 * extremeDifficultyMultiplier).TotalSeconds;
        var fastDecreaseFinalLastSecond = TimeSpan.FromMinutes(30 * extremeDifficultyMultiplier).TotalSeconds;

        // Initialize your Bezier curve with the control points
        BezierCurve curve = new BezierCurve(
            new Vector2((float)maxRealTimeScoreLastSecond, maxRealTimeScore),
            new Vector2(2000, (float)(maxRealTimeScore * 0.75)),
            new Vector2(2500, (float)(maxRealTimeScore * 0.50)),
            new Vector2(3000, (float)(maxRealTimeScore * 0.25))
        );

        // Calculate the elapsed time (in seconds)
        float elapsedTime = 3000;

        // Calculate the total time (in seconds)
        float totalTime = 10000;

        // Calculate the t parameter
        float t = elapsedTime / totalTime;

        // Calculate the score
        Vector2 scorePoint = curve.Evaluate(t);
        var score = scorePoint.Y;

        return score;
    }

    /// <summary>
    /// Calculates the bingo points obtained on the square.
    /// </summary>
    /// <param name="baseScore"></param>
    /// <returns>The bingo points obtained.</returns>
    public double CalculateBingoSquarePoints(int baseScore, int carts, double monsterTypeMultiplier, bool isMiddleSquare, bool weaponBonusActive)
    {
        // bingo points for a square = ((((base score + base score flat increase) * base score multiplier) + total carts score) * weapon multiplier * middle square multiplier)
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
        var totalCartsScore = CartsScore / cartsPenalty;
        
        var weaponMultiplier = weaponBonusActive ? WeaponMultiplier : 1;
        var middleSquareMultiplier = isMiddleSquare ? MiddleSquareMultiplier : 1;
        double score =
            (
                (
                    (
                        (baseScore + BaseScoreFlatIncrease)
                        * BaseScoreMultiplier
                    ) + totalCartsScore
                ) * weaponMultiplier * middleSquareMultiplier * monsterTypeMultiplier
            );
        return score;
    }

    /// <summary>
    /// Updates the upgrades values with the level value increase.
    /// </summary>
    /// <param name="upgrade"></param>
    public void ApplyUpgradeValue(BingoUpgrade upgrade)
    {
        if (BingoUpgradeValueProgressions.ValueProgressions.TryGetValue(upgrade.Type, out var valueProgression))
        {
            // Apply the upgrade's effects on the bingo game using the value progression
            double valueIncrease = valueProgression.CalculateLinearValueForLevel(upgrade.CurrentLevel);

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

    /// <summary>
    /// Starts a simulation of bingo runs.
    /// </summary>
    /// <param name="runs"></param>
    public void SimulateBingoRuns(int runs)
    {
        // TODO statistics
    }

    private BingoService() => LoggerInstance.Info($"Service initialized");
    private static readonly Logger LoggerInstance = LogManager.GetCurrentClassLogger();
    private static readonly DatabaseService DatabaseServiceInstance = DatabaseService.GetInstance();
    private static BingoService? instance;
}
