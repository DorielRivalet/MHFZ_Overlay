// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using MHFZ_Overlay.Models.Structures;

/// <summary>
/// TODO readonlydictionary of bingoupgrades model
/// </summary>
public sealed class BingoUpgrade
{
    /// <summary>
    /// The name of the upgrade.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the upgrade.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The icon of the upgrade.
    /// </summary>
    public string Icon { get; set; } = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png";

    /// <summary>
    /// The type of the bingo upgrade.
    /// </summary>
    public BingoUpgradeType Type { get; set; }

    /// <summary>
    /// The max level possible of the upgrade.
    /// </summary>
    public int MaxLevel { get; set; }

    /// <summary>
    /// The current level of the upgrade.
    /// </summary>
    public int CurrentLevel { get; set; }

    /// <summary>
    /// Whether the upgrade is unlocked or not.
    /// </summary>
    public bool IsUnlocked { get; set; }
}
