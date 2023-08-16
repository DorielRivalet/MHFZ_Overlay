// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services.Contracts;
using MHFZ_Overlay.Views.Windows;

public sealed class ChallengeService : IChallenge
{
    /// <inheritdoc/>
    public ChallengeState State { get; set; }

    public static ChallengeService GetInstance()
    {
        if (instance == null)
        {
            Logger.Debug(CultureInfo.InvariantCulture, "Singleton not found, creating instance.");
            instance = new ChallengeService();
        }

        Logger.Debug(CultureInfo.InvariantCulture, "Singleton found, returning instance.");
        Logger.Trace(CultureInfo.InvariantCulture, new StackTrace().ToString());
        return instance;
    }

    public static BingoGauntletCategory ConvertToBingoGauntletCategory(long category) => category switch
    {
        0 => BingoGauntletCategory.Unknown,
        1 => BingoGauntletCategory.GreatSword,
        2 => BingoGauntletCategory.LongSword,
        3 => BingoGauntletCategory.DualSwords,
        4 => BingoGauntletCategory.SwordAndShield,
        5 => BingoGauntletCategory.Hammer,
        6 => BingoGauntletCategory.HuntingHorn,
        7 => BingoGauntletCategory.Lance,
        8 => BingoGauntletCategory.Gunlance,
        9 => BingoGauntletCategory.LightBowgun,
        10 => BingoGauntletCategory.HeavyBowgun,
        11 => BingoGauntletCategory.Bow,
        12 => BingoGauntletCategory.Tonfa,
        13 => BingoGauntletCategory.SwitchAxeF,
        14 => BingoGauntletCategory.MagnetSpike,
        15 => BingoGauntletCategory.Multiple,
        16 => BingoGauntletCategory.Multiplayer,
        _ => BingoGauntletCategory.Unknown,
    };

    /// <inheritdoc/>
    public bool CheckRequirements(Challenge challenge)
    {
        var playerAchievements = DatabaseServiceInstance.GetPlayerAchievements();

        var obtainedBronzeAchievements = playerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Bronze);
        var obtainedSilverAchievements = playerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Silver);
        var obtainedGoldAchievements = playerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Gold);
        var obtainedPlatinumAchievements = playerAchievements.Count(a => a.CompletionDate != DateTime.UnixEpoch && a.Rank == AchievementRank.Platinum);
        var obtainedSecretAchievement = playerAchievements.Any(achievement => achievement.CompletionDate != DateTime.UnixEpoch && achievement.Title == challenge.AchievementNameRequired && achievement.IsSecret);

        return (obtainedBronzeAchievements >= challenge.AchievementsBronzeRequired &&
            obtainedSilverAchievements >= challenge.AchievementsSilverRequired &&
            obtainedGoldAchievements >= challenge.AchievementsGoldRequired &&
            obtainedBronzeAchievements >= challenge.AchievementsPlatinumRequired &&
            obtainedSecretAchievement);
    }

    /// <inheritdoc/>
    public bool Unlock(Challenge challenge)
    {
        if (!CheckChallengeWindow(challenge))
        {
            return false;
        }

        // Find the challenge ID by iterating through the ReadOnlyDictionary
        foreach (var kvp in Challenges.IDChallenge)
        {
            if (kvp.Value == challenge)
            {
                var challengeID = kvp.Key;
                return DatabaseServiceInstance.UnlockChallenge(challenge, challengeID);
            }
        }

        Logger.Error(CultureInfo.InvariantCulture, "Challenge not found in the dictionary, canceling unlock process.");
        return false;
    }

    /// <inheritdoc/>
    public bool Start(Challenge challenge)
    {
        if (State == ChallengeState.Running)
        {
            return false;
        }
        else
        {
            var success = OpenChallengeWindow(challenge);

            if (success)
            {
                State = ChallengeState.Running;
            }

            return success;
        }
    }

    /// <inheritdoc/>
    public bool OpenChallengeWindow(Challenge challenge)
    {
        switch (challenge.Name)
        {
            default:
                Logger.Error(CultureInfo.InvariantCulture, "Could not find window for challenge {0}. The challenge has yet to be made by the developer.", challenge.Name);
                return false;
            case "Bingo":
                var window = new BingoWindow();
                window.Show();
                return true;
        }
    }

    /// <summary>
    /// Checks if there exists a window for the challenge.
    /// </summary>
    /// <param name="challenge"></param>
    /// <returns>False if a window could not be found for the selected challenge.</returns>
    public bool CheckChallengeWindow(Challenge challenge)
    {
        switch (challenge.Name)
        {
            default:
                Logger.Info(CultureInfo.InvariantCulture, "Could not find window for challenge {0}. The challenge has yet to be made by the developer.", challenge.Name);
                return false;
            case "Bingo":
                return true;
        }
    }

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private static readonly DatabaseService DatabaseServiceInstance = DatabaseService.GetInstance();

    private static ChallengeService? instance;

    private ChallengeService() => Logger.Info(CultureInfo.InvariantCulture, $"Service initialized");
}
