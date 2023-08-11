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

public sealed class ChallengeService : IChallenge
{
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
        // TODO
        return false;
    }

    /// <inheritdoc/>
    public Challenge Cancel(Challenge challenge)
    {
        // TODO use enum?
        return challenge;
    }

    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    private static readonly DatabaseService DatabaseServiceInstance = DatabaseService.GetInstance();

    private static ChallengeService? instance;

    private ChallengeService() => Logger.Info(CultureInfo.InvariantCulture, $"Service initialized");
}
