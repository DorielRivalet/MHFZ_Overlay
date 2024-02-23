// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Memory;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

/// <summary>
/// The challenge ancient dragon.
/// </summary>
public sealed class ChallengeAncientDragon
{
    /// <summary>
    /// The parts of the dragon.
    /// </summary>
    public List<ChallengeAncientDragonPart>? Parts { get; set; }

    /// <summary>
    /// The name of the dragon.
    /// </summary>
    public string Name { get; set; } = "Ancient Dragon";

    /// <summary>
    /// Whether the dragon is active or not.
    /// </summary>
    public bool IsActive { get; set; }
}
