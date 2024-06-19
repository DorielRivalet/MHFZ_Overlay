// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;
using System.Windows;
using Wpf.Ui.Controls;

public interface IAchievementService
{
    void LoadPlayerAchievements();

    /// <summary>
    /// Rewards mutliple achievements.
    /// </summary>
    /// <param name="snackbarPresenter"></param>
    /// <param name="dataLoader"></param>
    /// <param name="databaseManagerInstance"></param>
    /// <param name="s"></param>
    /// <param name="style"></param>
    void CheckForAchievements(SnackbarPresenter snackbarPresenter, DataLoader dataLoader, DatabaseService databaseManagerInstance, Settings s, Style style);

    /// <summary>
    /// Rewards a single achievement.
    /// </summary>
    /// <param name="achievementID"></param>
    /// <param name="snackbar"></param>
    /// <param name="style"></param>
    void RewardAchievement(int achievementID, Snackbar snackbar, Style style);
}
