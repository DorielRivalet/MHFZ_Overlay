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
/// The challenge item crafting table.
/// </summary>
public sealed class ChallengeItemCraftingTable
{
    /// <summary>
    /// The slots of the crafting table.
    /// </summary>
    public ChallengeItemSlot[,] Slots { get; set; }

    /// <summary>
    /// The result of the craft.
    /// </summary>
    public ChallengeItem? Result { get; set; }

    public ChallengeItemCraftingTable(int size)
    {
        Slots = new ChallengeItemSlot[size, size];
    }

    public void UpdateResult()
    {
        // Determine what item can be crafted from the current arrangement of items in the Slots array
        // and set the Result property to that item
        // ...
    }
}
