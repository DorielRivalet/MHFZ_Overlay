// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Views.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;

/// <summary>
/// The achievements of the player.
/// </summary>
public sealed class Achievement
{
    public static async Task ShowMany(Snackbar snackbar, List<int> achievementsID)
    {
        const int maxAchievementsToShow = 5;
        var remainingAchievements = achievementsID.Count - maxAchievementsToShow;

        foreach (var achievementID in achievementsID.Take(maxAchievementsToShow))
        {
            if (Achievements.IDAchievement.TryGetValue(achievementID, out Achievement? achievement) && achievement != null)
            {
                await achievement.Show(snackbar);
                await Task.Delay(TimeSpan.FromSeconds(2)); // Delay between each achievement
            }
        }

        if (remainingAchievements > 0)
        {
            await ShowAchievementsTabInfo(snackbar, remainingAchievements);
        }
    }

    public async Task Show(Snackbar snackbar)
    {
        var brushColor = this.GetBrushColorFromRank();
        if (brushColor == null)
        {
            brushColor = Brushes.Black;
        }

        snackbar.Title = this.Title;
        snackbar.Message = this.Objective;
        snackbar.Icon = new SymbolIcon()
        {
            Symbol = SymbolRegular.Trophy32,
        };
        snackbar.Icon.Foreground = brushColor;
        snackbar.Appearance = ControlAppearance.Secondary;
        if (MainWindow.MainWindowSoundPlayer != null)
        {
            MainWindow.MainWindowSoundPlayer.Play();
        }

        await snackbar.ShowAsync();
        await Task.Delay(TimeSpan.FromSeconds(1)); // Delay for a certain duration
        snackbar.Hide(); // Hide the snackbar
    }

    public static async Task ShowAchievementsTabInfo(Snackbar snackbar, int remainingAchievements)
    {
        var brushConverter = new BrushConverter();
        var brushColor = (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Crust"]);
        snackbar.Title = "Too many achievements!";
        snackbar.Message = $"To see the rest of the achievements unlocked ({remainingAchievements} left), see the Achievements tab in the Quests Logs section.";
        snackbar.Icon = new SymbolIcon()
        {
            Symbol = SymbolRegular.Info28,
        };
        snackbar.Icon.Foreground = brushColor ?? Brushes.Black;
        snackbar.Appearance = ControlAppearance.Info;
        await snackbar.ShowAsync();
        await Task.Delay(TimeSpan.FromSeconds(1)); // Delay for a certain duration
        snackbar.Hide(); // Hide the snackbar
    }

    /// <summary>
    /// Gets the color for title and icon from rank.
    /// </summary>
    public Brush? GetBrushColorFromRank()
    {
        var brushConverter = new BrushConverter();

        if (RankColors.TryGetValue(this.Rank, out var colorString))
        {
            if (colorString == null)
            {
                colorString = CatppuccinMochaColors.NameHex["Base"];
            }

            var brush = (Brush?)brushConverter.ConvertFromString(colorString);
            return brush;
        }

        // Default color if rank is not defined
        return (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColors.NameHex["Base"]);
    }

    public string GetTrophyImageLinkFromRank()
    {
        if (this.IsSecret && this.CompletionDate == DateTime.UnixEpoch)
        {
            return "pack://application:,,,/Assets/Icons/achievement/secret_trophy.png";
        }

        switch(this.Rank)
        {
            default:
                return "pack://application:,,,/Assets/Icons/achievement/bronze_trophy.png";
            case AchievementRank.Bronze:
                return "pack://application:,,,/Assets/Icons/achievement/bronze_trophy.png";
            case AchievementRank.Silver:
                return "pack://application:,,,/Assets/Icons/achievement/silver_trophy.png";
            case AchievementRank.Gold:
                return "pack://application:,,,/Assets/Icons/achievement/gold_trophy.png";
            case AchievementRank.Platinum:
                return "pack://application:,,,/Assets/Icons/achievement/platinum_trophy.png";
        }
    }

    /// <summary>
    /// Gets or sets the completion date.
    /// </summary>
    /// <value>
    /// The completion date.
    /// </value>
    public DateTime CompletionDate { get; set; } = DateTime.UnixEpoch;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; } = "Achievement Obtained!";

    /// <summary>
    /// Gets or sets the description of the achivement (flavor text).
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the achievement rank.
    /// </summary>
    /// <value>
    /// The achievement rank.
    /// </value>
    public AchievementRank Rank { get; set; } = AchievementRank.None;

    /// <summary>
    /// Gets or sets the objective description to obtain this achievement.
    /// </summary>
    /// <value>
    /// The objective.
    /// </value>
    public string Objective { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image of the achievement when displayed.
    /// </summary>
    /// <value>
    /// The image.
    /// </value>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is secret. If it is secret, it shows everything as ?, otherwise it shows the Objective and everything else but grayed out.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is secret; otherwise, <c>false</c>.
    /// </value>
    public bool IsSecret { get; set; }

    /// <summary>
    /// Gets or sets the hint, which replaces the Objective if the achievement is secret.
    /// </summary>
    /// <value>
    /// The hint.
    /// </value>
    public string Hint { get; set; } = string.Empty;

    // Additional properties or methods related to achievements can be added here
    private static readonly Dictionary<AchievementRank, string> RankColors = new Dictionary<AchievementRank, string>
    {
        { AchievementRank.None, CatppuccinMochaColors.NameHex["Base"] },        // Black
        { AchievementRank.Bronze, CatppuccinMochaColors.NameHex["Maroon"] },      // Bronze color
        { AchievementRank.Silver, CatppuccinMochaColors.NameHex["Lavender"] },      // Silver color
        { AchievementRank.Gold, CatppuccinMochaColors.NameHex["Yellow"] },        // Gold color
        { AchievementRank.Platinum, CatppuccinMochaColors.NameHex["Teal"] },     // Platinum color
    };
}
