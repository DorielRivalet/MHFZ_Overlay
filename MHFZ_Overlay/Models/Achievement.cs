// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

/// <summary>
/// The achievements of the player.
/// </summary>
public sealed class Achievement
{
    // Additional properties or methods related to achievements can be added here
    private static readonly Dictionary<AchievementRank, string> RankColors = new ()
    {
        { AchievementRank.None, CatppuccinMochaColors.NameHex["Base"] },        // Black
        { AchievementRank.Bronze, CatppuccinMochaColors.NameHex["Maroon"] },      // Bronze color
        { AchievementRank.Silver, CatppuccinMochaColors.NameHex["Lavender"] },      // Silver color
        { AchievementRank.Gold, CatppuccinMochaColors.NameHex["Yellow"] },        // Gold color
        { AchievementRank.Platinum, CatppuccinMochaColors.NameHex["Teal"] },     // Platinum color
    };

    public TimeSpan SnackbarTimeOut { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets the completion date.
    /// </summary>
    /// <value>
    /// The completion date.
    /// </value>
    public DateTime CompletionDate { get; set; } = DateTime.UnixEpoch;

    public void Show(Snackbar snackbar, Style style)
    {
        var brushColor = this.GetBrushColorFromRank();
        brushColor ??= Brushes.Black;
        snackbar.Style = style;
        snackbar.Title = this.Title;
        snackbar.Content = this.Objective;
        snackbar.Icon = new SymbolIcon
        {
            Symbol = SymbolRegular.Trophy32,
            Foreground = brushColor,
        };
        snackbar.Appearance = ControlAppearance.Secondary;
        var s = (Settings)Application.Current.TryFindResource("Settings");
        var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\Sounds\victory.wav");
        AudioService.GetInstance().Play(fileName, MainWindow.MainWindowMediaPlayer, s.VolumeMain, s.VolumeAchievementUnlock);
        snackbar.Timeout = this.SnackbarTimeOut;
        snackbar.Show();
    }

    /// <summary>
    /// Gets the color for title and icon from rank.
    /// </summary>
    /// <returns></returns>
    public Brush? GetBrushColorFromRank()
    {
        var brushConverter = new BrushConverter();

        if (RankColors.TryGetValue(this.Rank, out var colorString))
        {
            colorString ??= CatppuccinMochaColors.NameHex["Base"];

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

        return this.Rank switch
        {
            AchievementRank.Bronze => "pack://application:,,,/Assets/Icons/achievement/bronze_trophy.png",
            AchievementRank.Silver => "pack://application:,,,/Assets/Icons/achievement/silver_trophy.png",
            AchievementRank.Gold => "pack://application:,,,/Assets/Icons/achievement/gold_trophy.png",
            AchievementRank.Platinum => "pack://application:,,,/Assets/Icons/achievement/platinum_trophy.png",
            AchievementRank.None => "pack://application:,,,/Assets/Icons/achievement/bronze_trophy.png",
            _ => "pack://application:,,,/Assets/Icons/achievement/bronze_trophy.png",
        };
    }

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

    /// <summary>
    /// Whether the achievement is unused or not. Hidden in views and does not count towards progress.
    /// </summary>
    public bool? Unused { get; set; } = false;
}
