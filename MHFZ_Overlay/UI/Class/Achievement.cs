using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;

namespace MHFZ_Overlay.UI.Class;

/// <summary>
/// TODO: add GHC carve sound when obtaining and displaying snackbar
/// </summary>
public class Achievement
{
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
    /// Gets or sets the snackbar icon.
    /// </summary>
    /// <value>
    /// The icon.
    /// </value>
    public IconElement Icon { get; set; } = new SymbolIcon(SymbolRegular.Fluent24);
    /// <summary>
    /// Gets or sets the appearance.
    /// </summary>
    /// <value>
    /// The appearance.
    /// </value>
    public ControlAppearance Appearance { get; set; } = ControlAppearance.Secondary;
    /// <summary>
    /// Gets or sets the completion date.
    /// </summary>
    /// <value>
    /// The completion date.
    /// </value>
    public DateTime CompletionDate { get; set; } = DateTime.MinValue;
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
    /// <summary>
    /// Gets or sets a value indicating whether this instance is unlocked.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is unlocked; otherwise, <c>false</c>.
    /// </value>
    public bool IsUnlocked { get; set; } = false;
    // Additional properties or methods related to achievements can be added here
}
