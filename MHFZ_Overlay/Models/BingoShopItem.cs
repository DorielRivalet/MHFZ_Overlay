// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The bingo shop options.
/// </summary>
public sealed class BingoShopItem
{
    /// <summary>
    /// The name of the option.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The cost of the option.
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    /// Whether the option is unlocked.
    /// </summary>
    public bool IsUnlocked { get; set; }
}
