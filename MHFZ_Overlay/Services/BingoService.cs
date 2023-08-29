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
using LiveChartsCore.Geo;
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
    public decimal BaseScoreMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.BaseScoreFlatIncrease"/>
    public decimal BaseScoreFlatIncrease { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.WeaponMultiplier"/>
    public decimal WeaponMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.MiddleSquareMultiplier"/>
    public decimal MiddleSquareMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.BonusScore"/>
    public decimal BonusScore { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.CartsScore"/>
    public decimal CartsScore { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.AchievementMultiplier"/>
    public decimal AchievementMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.SecretAchievementFlatIncrease"/>
    public decimal SecretAchievementFlatIncrease { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.BingoCompletionsMultiplier"/>
    public decimal BingoCompletionsMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.ZenithMultiplier"/>
    public decimal ZenithMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.SolsticeMultiplier"/>
    public decimal SolsticeMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.MusouMultiplier"/>
    public decimal MusouMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.HorizontalLineCompletionMultiplier"/>
    public decimal HorizontalLineCompletionMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.VerticalLineCompletionMultiplier"/>
    public decimal VerticalLineCompletionMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.DiagonalLineCompletionMultiplier"/>
    public decimal DiagonalLineCompletionMultiplier { get; set; }

    /// <inheritdoc cref="BingoUpgradeType.RealTimeMultiplier"/>
    public decimal RealTimeMultiplier { get; set; }

    /// <summary>
    /// Gets the player bingo points. Used for relegation. The data flow is View <-> ViewModel <-> Service <-> Database
    /// </summary>
    /// <returns>The player bingo points.</returns>
    public long GetPlayerBingoPoints() => DatabaseServiceInstance.GetPlayerBingoPoints();

    /// <summary>
    /// Spends the bingo points according to the cost.
    /// </summary>
    /// <param name="cost"></param>
    /// <returns>false if could not spend points.</returns>
    public bool SpendBingoPoints(long cost)
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
            var nextLevel = upgrade.CurrentLevel + 1;
            var nextCost = costProgression.CalculateLinearValueForLevel(nextLevel);
            var playerBingoPoints = GetPlayerBingoPoints();

            if (playerBingoPoints < nextCost)
            {
                return false;
            }

            playerBingoPoints -= (long)nextCost;
            DatabaseServiceInstance.SetPlayerBingoPoints(playerBingoPoints);
            upgrade.CurrentLevel++;
            ApplyUpgradeValue(upgrade);
            return true;
        }
        else
        {
            var costProgression = BingoUpgradeCostProgressions.ExponentialCostProgressions[upgrade.Type];
            var nextLevel = upgrade.CurrentLevel + 1;
            var nextCost = costProgression.CalculateExponentialValueForLevel(nextLevel);
            var playerBingoPoints = GetPlayerBingoPoints();

            if (playerBingoPoints < nextCost)
            {
                return false;
            }

            playerBingoPoints -= (long)nextCost;
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
        var monsterTypeMultiplier = 1M;
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
        var bingoLineTypeMultiplier = 1M;
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

        // TODO number of lines crossing, max 4.
        var linesCrossing = 4;
        var totalSquaresBingoPoints = squares * squareBingoPoints;

        var extremeDifficultyMultiplier = difficulty == Difficulty.Extreme ? 2 : 1;
        var obtainedAchievementsMultiplier = 1 + (AchievementMultiplier * obtainedAchievements);
        var obtainedSecretAchievementsScore = SecretAchievementFlatIncrease * obtainedSecretAchievements;

        // TODO
        decimal bingoCompletionsLogMultiplier = (decimal)Math.Log2((double)(1 + (bingoCompletions * BingoCompletionsMultiplier)));

        // TODO
        var elapsedRealTimeInSeconds = 0;
        var elapsedRealTimeInHours = elapsedRealTimeInSeconds / 3600M;

        // TODO
        // f(x) = 1/(e^x) where x is the hours elapsed.
        var realTimeScoreMultiplier = (1M + (1M / (decimal)Math.Pow(Math.E, (double)elapsedRealTimeInHours)) * (RealTimeMultiplier * extremeDifficultyMultiplier)); // max multi at infinite time tends to 4. max multi is 8.
        var maxRealTimeScore = BingoStartCosts.DifficultyCosts[difficulty];
        decimal maxRealTimeScoreSecondsLimit = (decimal)Math.Ceiling(TimeSpan.FromMinutes(10 * extremeDifficultyMultiplier).TotalSeconds);

        decimal realTimeScore = 0;
        // TODO maybe remove grace period. or make it an upgrade.
        if (elapsedRealTimeInSeconds <= maxRealTimeScoreSecondsLimit)
        {
            realTimeScore = maxRealTimeScore;
        }
        else
        {
            realTimeScore = CalculateRealTimeScore(elapsedRealTimeInSeconds, realTimeScoreMultiplier, maxRealTimeScore, extremeDifficultyMultiplier);
        }

        // TODO
        // bingo ponts for final score = (total squares bingo points * extreme difficulty multiplier) +  + bonus score
        // TODO see which one of the 2 affects the score the most (bingoBoardScore vs extraScore)
        var bingoBoardScore = totalSquaresBingoPoints * linesCrossing * extremeDifficultyMultiplier;
        var extraScore = realTimeScore * bingoCompletionsLogMultiplier * obtainedAchievementsMultiplier;
        var finalScore = bingoBoardScore + extraScore + obtainedSecretAchievementsScore + BonusScore;
        return (int)Math.Ceiling(finalScore);
    }

    public decimal CalculateRealTimeScore(int elapsedRealTimeInSeconds, decimal realTimeScoreMultiplier, int maxRealTimeScore, int extremeDifficultyMultiplier)
    {
        var maxRealTimeScoreMinuteLimit = 10 * extremeDifficultyMultiplier;
        var maxRealTimeScoreLastSecond = TimeSpan.FromMinutes(maxRealTimeScoreMinuteLimit).TotalSeconds;
        var fastDecreaseFromMaxScoreLastSecond = TimeSpan.FromMinutes(maxRealTimeScoreMinuteLimit * 1.2).TotalSeconds;
        var slowDecreaseFromHalvedScoreLastSecond = TimeSpan.FromMinutes(maxRealTimeScoreMinuteLimit * 1.4).TotalSeconds;
        var fastDecreaseFinalLastSecond = TimeSpan.FromMinutes(maxRealTimeScoreMinuteLimit * 1.6).TotalSeconds;

        // Initialize your Bezier curve with the control points
        //BezierCurve curve = new BezierCurve(
        //    new Vector2((float)maxRealTimeScoreLastSecond, maxRealTimeScore),
        //    new Vector2((float)fastDecreaseFromMaxScoreLastSecond, (float)(maxRealTimeScore * 0.75)),
        //    new Vector2((float)slowDecreaseFromHalvedScoreLastSecond, (float)(maxRealTimeScore * 0.50)),
        //    new Vector2((float)fastDecreaseFinalLastSecond, (float)(maxRealTimeScore * 0.25))
        //);

        BezierCurve curve = new BezierCurve(
            new Vector2(2 * 60 * 60, 0),
            new Vector2(2 * 60 * 60, 1000),
            new Vector2(10 * 60, 0),
            new Vector2(10 * 60, 1000)
        );

        // Calculate the elapsed time (in seconds)
        float elapsedTime = 3000;

        // Calculate the total time (in seconds)
        float totalTime = 10000;

        // Calculate the t parameter
        float t = Math.Min(elapsedTime / totalTime, 1);

        // Calculate the score
        Vector2 scorePoint = curve.Evaluate(t);
        var score = scorePoint.Y;

        return (decimal)score;
    }

    /// <summary>
    /// Calculates the bingo points obtained on the square.
    /// </summary>
    /// <param name="baseScore"></param>
    /// <returns>The bingo points obtained.</returns>
    public decimal CalculateBingoSquarePoints(int baseScore, int carts, decimal monsterTypeMultiplier, bool isMiddleSquare, bool weaponBonusActive)
    {
        // bingo points for a square = ((((base score + base score flat increase) * base score multiplier) + total carts score) * weapon multiplier * middle square multiplier)
        var cartsPenalty = 1M;
        switch (carts)
        {
            case 1:
                cartsPenalty = 2M;
                break;
            case >= 2:
                cartsPenalty = 3M;
                break;
        }
        var totalCartsScore = CartsScore / cartsPenalty;
        
        var weaponMultiplier = weaponBonusActive ? WeaponMultiplier : 1;
        var middleSquareMultiplier = isMiddleSquare ? MiddleSquareMultiplier : 1;
        var score =
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
            decimal valueIncrease = valueProgression.CalculateLinearValueForLevel(upgrade.CurrentLevel);

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

    /// <summary>
    /// TODO upgrades affecting cost.
    /// </summary>
    /// <param name="gauntletBoost"></param>
    /// <param name="difficulty"></param>
    /// <param name="musouElzelionBoost"></param>
    /// <returns></returns>
    public int CalculateBingoStartCost(GauntletBoost gauntletBoost, Difficulty difficulty, bool musouElzelionBoost)
    {
        var cost = 0;

        // Base cost based on difficulty
        if (BingoStartCosts.DifficultyCosts.TryGetValue(difficulty, out var difficultyCost))
        {
            cost = difficultyCost;
        }

        // Additional cost based on gauntlet boost
        if (gauntletBoost.HasFlag(GauntletBoost.Zenith))
        {
            cost += BingoStartCosts.GauntletBoostCosts[GauntletBoost.Zenith];
        }

        if (gauntletBoost.HasFlag(GauntletBoost.Solstice))
        {
            cost += BingoStartCosts.GauntletBoostCosts[GauntletBoost.Solstice];
        }

        if (gauntletBoost.HasFlag(GauntletBoost.Musou))
        {
            cost += BingoStartCosts.GauntletBoostCosts[GauntletBoost.Musou];
        }

        // Additional cost based on Musou Elzelion boost
        if (musouElzelionBoost)
        {
            cost += BingoStartCosts.MusouElzelionBoostCost;
        }

        // Additional cost based on number of players
        // cost += numberOfPlayers * 20;
        return cost;
    }

    /// <summary>
    /// Calculates the carts depending on difficulty selected, for bingo start.
    /// </summary>
    /// <param name="difficulty"></param>
    /// <returns></returns>
    public int CalculateCartsAtBingoStartFromSelectedDifficulty(Difficulty difficulty)
    {
        var carts = 0;

        // Base carts based on difficulty
        if (BingoDifficultyCarts.DifficultyCarts.TryGetValue(difficulty, out var difficultyCarts))
        {
            carts = difficultyCarts;
        }

        return carts;
    }

    /// <summary>
    /// For testing only. TODO add for challenge unlocks.
    /// </summary>
    /// <param name="points"></param>
    public long SetPlayerBingoPoints(long points)
    {
        DatabaseServiceInstance.SetPlayerBingoPoints(points);
        return points;
    }

    private BingoService() => LoggerInstance.Info($"Service initialized");
    private static readonly Logger LoggerInstance = LogManager.GetCurrentClassLogger();
    private static readonly DatabaseService DatabaseServiceInstance = DatabaseService.GetInstance();
    private static BingoService? instance;
}
