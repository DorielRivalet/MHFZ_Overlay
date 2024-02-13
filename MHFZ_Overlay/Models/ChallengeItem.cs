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
/// The challenge item.
/// </summary>
public sealed class ChallengeItem
{
    /// <summary>
    /// The name of the item
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the item.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The link of the item image.
    /// </summary>
    public string ImageLink { get; set; } = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png";

    /// <summary>
    /// Whether the item can be stacked.
    /// </summary>
    public bool IsStackable { get; set; }

    /// <summary>
    /// The max stack size in an inventory grid cell for the item.
    /// </summary>
    public int MaxStackSize { get; set; }

    /// <summary>
    /// Whether there can be more than 1 of the item in the inventory.
    /// </summary>
    public bool IsUnique { get; set; }

    /// <summary>
    /// The rarity of the item. From 1 to 12.
    /// </summary>
    public int Rarity { get; set; }
}
