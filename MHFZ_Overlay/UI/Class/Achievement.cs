using MHFZ_Overlay.Core.Class.Dictionary;
using MHFZ_Overlay.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;

namespace MHFZ_Overlay.UI.Class;

/// <summary>
/// TODO: add sound when obtaining and displaying snackbar
/// </summary>
public class Achievement
{
    private static readonly Dictionary<AchievementRank, string> RankColors = new Dictionary<AchievementRank, string>
    {
        { AchievementRank.None, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Base"] },        // Black
        { AchievementRank.Bronze, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Maroon"] },      // Bronze color
        { AchievementRank.Silver, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Lavender"] },      // Silver color
        { AchievementRank.Gold, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Yellow"] },        // Gold color
        { AchievementRank.Platinum, CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Teal"] }     // Platinum color
    };

    public void Show() 
    { 
        // TODO sound, async, sequence
    }

    /// <summary>
    /// Gets the color for title and icon from rank.
    /// </summary>
    public Brush? GetBrushColorFromRank()
    {
        var brushConverter = new BrushConverter();

        if (RankColors.TryGetValue(Rank, out string? colorString))
        {
            if (colorString == null)
                colorString = CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Base"];
            var brush = (Brush?)brushConverter.ConvertFromString(colorString);
            return brush;
        }
        // Default color if rank is not defined
        return (Brush?)brushConverter.ConvertFromString(CatppuccinMochaColorsDictionary.CatppuccinMochaColors["Base"]); 
    }

    /// <summary>
    /// Gets or sets the completion date.
    /// </summary>
    /// <value>
    /// The completion date.
    /// </value>
    public DateTime CompletionDate { get; set; } = DateTime.MinValue;
    /// <summary>
    /// Gets or sets a value indicating whether this instance is unlocked.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is unlocked; otherwise, <c>false</c>.
    /// </value>
    public bool IsUnlocked { get; set; } = false;
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the description of the snackbar.
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
    public bool IsSecret { get; set; } = false;
    /// <summary>
    /// Gets or sets the hint, which replaces the Objective if the achievement is secret.
    /// </summary>
    /// <value>
    /// The hint.
    /// </value>
    public string Hint { get; set; } = string.Empty;
    // Additional properties or methods related to achievements can be added here
}
