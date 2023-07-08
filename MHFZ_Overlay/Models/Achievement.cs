// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using MHFZ_Overlay.Models.Mappers;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Views;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;

/// <summary>
/// The achievements of the player.
/// </summary>
public class Achievement
{
    public static async Task ShowMany(Snackbar snackbar, List<int> achievementsID)
    {
        foreach (var achievementID in achievementsID)
        {
            if (AchievementsMapper.IDAchievement.TryGetValue(achievementID, out Achievement? achievement))
            {
                if (achievement == null)
                {
                    continue;
                }

                await achievement.Show(snackbar);
                await Task.Delay(TimeSpan.FromSeconds(2)); // Delay between each achievement
            }
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
        snackbar.Icon = new SymbolIcon(SymbolRegular.Trophy32);
        snackbar.Icon.Foreground = brushColor;
        snackbar.Appearance = ControlAppearance.Secondary;
        if (MainWindow.victoryMediaSound != null)
        {
            MainWindow.victoryMediaSound.Play();
        }

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
                colorString = CatppuccinMochaColorsMapper.CatppuccinMochaColors["Base"];
            }

            var brush = (Brush?)brushConverter.ConvertFromString(colorString);
            return brush;
        }

        // Default color if rank is not defined
        return (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColorsMapper.CatppuccinMochaColors["Base"]);
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
        { AchievementRank.None, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Base"] },        // Black
        { AchievementRank.Bronze, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Maroon"] },      // Bronze color
        { AchievementRank.Silver, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Lavender"] },      // Silver color
        { AchievementRank.Gold, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Yellow"] },        // Gold color
        { AchievementRank.Platinum, CatppuccinMochaColorsMapper.CatppuccinMochaColors["Teal"] },     // Platinum color
    };
}
