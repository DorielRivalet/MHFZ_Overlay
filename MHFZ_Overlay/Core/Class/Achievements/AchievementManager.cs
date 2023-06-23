// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay.Core.Class.DataAccessLayer;
using MHFZ_Overlay.Core.Class.Dictionary;
using MHFZ_Overlay.UI.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace MHFZ_Overlay.Core.Class.Achievements;

public class AchievementManager
{
    private static AchievementManager? instance;
    private static readonly DatabaseManager databaseManager = DatabaseManager.GetInstance();

    private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    public HashSet<int> obtainedAchievements = new HashSet<int>();
    private AchievementManager()
    {
        logger.Info($"AchievementManager initialized");
    }

    public static AchievementManager GetInstance()
    {
        if (instance == null)
        {
            logger.Debug("Singleton not found, creating instance.");
            instance = new AchievementManager();
        }
        logger.Debug("Singleton found, returning instance.");
        logger.Trace(new StackTrace().ToString());
        return instance;
    }

    public void LoadPlayerAchievements()
    {
        List<int> playerAchievements = databaseManager.GetPlayerAchievementIDList();

        foreach (int achievementID in playerAchievements)
        {
            obtainedAchievements.Add(achievementID);
        }
    }

    public async Task CheckForAchievementsAsync(Snackbar snackbar)
    {
        List<int> newAchievements = GetNewlyObtainedAchievements();

        if (newAchievements.Count > 0)
        {
            UpdatePlayerAchievements(newAchievements);
            await Achievement.ShowMany(snackbar, newAchievements);
        }
    }

    private List<int> GetNewlyObtainedAchievements()
    {
        List<int> newAchievements = new List<int>();

        foreach (var kvp in AchievementsDictionary.IDAchievement)
        {
            int achievementID = kvp.Key;
            Achievement achievement = kvp.Value;

            // Check the specific conditions for obtaining the achievement
            if (!obtainedAchievements.Contains(achievementID) && CheckConditionsForAchievement(achievementID))
            {
                newAchievements.Add(achievementID);
            }
        }

        return newAchievements;
    }

    private bool CheckConditionsForAchievement(int achievementID)
    {
        // Implement your logic here to check the conditions for obtaining the achievement
        // Return true if the conditions are met, false otherwise
        // You can access the properties of the achievement object to perform the checks
        return false;
    }

    private void UpdatePlayerAchievements(List<int> achievementsID)
    {
        // Update the player achievements table in the database with the newly obtained achievements
        // Use the provided database update logic or similar approach

        foreach (int achievementID in achievementsID)
        {
            obtainedAchievements.Add(achievementID);
            // Store the achievement in the SQLite PlayerAchievements table
            databaseManager.StoreAchievement(achievementID);
        }
    }

    public async Task RewardAchievement(int achievementID, Snackbar snackbar)
    {
        AchievementsDictionary.IDAchievement.TryGetValue(achievementID, out Achievement? achievement);
        if (achievement == null) return;

        if (!obtainedAchievements.Contains(achievementID))
        {
            obtainedAchievements.Add(achievementID);
            // Store the achievement in the SQLite PlayerAchievements table
            databaseManager.StoreAchievement(achievementID);
            await achievement.Show(snackbar);
            logger.Info("Awarded achievement ID {0}", achievementID);
        }
        else
        {
            logger.Warn("Achievement ID {0} already found", achievementID);
        }
    }
}
