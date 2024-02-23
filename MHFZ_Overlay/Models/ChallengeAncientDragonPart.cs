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
/// The challenge ancient dragon part.
/// </summary>
public sealed class ChallengeAncientDragonPart
{
    /// <summary>
    /// The name of the part.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the inactive part.
    /// </summary>
    public string InactivePartDescription { get; set; } = string.Empty;

    /// <summary>
    /// The description of the active part.
    /// </summary>
    public string ActivePartDescription { get; set; } = string.Empty;

    /// <summary>
    /// The link of the part image before a true transcend.
    /// </summary>
    public string InactivePartImageLink { get; set; } = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png";

    /// <summary>
    /// The link of the part image after a true transcend.
    /// </summary>
    public string ActivePartImageLink { get; set; } = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png";

    /// <summary>
    /// Whether the part boosts all other parts (except itself).
    /// </summary>
    public bool IsSource { get; set; }

    /// <summary>
    /// The extra bonus when having the part that synergizes with this part.
    /// </summary>
    public ChallengeAncientDragonPart? NextSynergyPart { get; set; }

    /// <summary>
    /// The description of the effect.
    /// </summary>
    public string Effect { get; set; } = string.Empty;

    /// <summary>
    /// The description of the extra effect if having the Source part already.
    /// </summary>
    public string SourceEffect { get; set; } = string.Empty;

    /// <summary>
    /// The description of the synergy effect if having the part that synergizes with this part.
    /// </summary>
    public string SynergyEffect { get; set; } = string.Empty;

    /// <summary>
    /// Any additional details e.g. show how many bingo points you keep after a true transcend.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// The amount of gems required from monster types to make a scrap. Each part requires 100 scraps.
    /// </summary>
    public Dictionary<FrontierMonsterType, int>? GemsRequiredForScrap { get; set; }
}
