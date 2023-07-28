// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Contracts;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;

public interface IAchievementService
{
    void LoadPlayerAchievements();

    void CheckForAchievements(SnackbarPresenter snackbarPresenter, DataLoader dataLoader, DatabaseService databaseManagerInstance, Settings s, Style style);

    void RewardAchievement(int achievementID, Snackbar snackbar, Style style);
}
