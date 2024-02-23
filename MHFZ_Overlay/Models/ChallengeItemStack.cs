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
/// The challenge item stack.
/// </summary>
public sealed class ChallengeItemStack
{
    public ChallengeItem? Item { get; set; }
    public int Quantity { get; set; }
}
