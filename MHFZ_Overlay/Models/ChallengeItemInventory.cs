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
/// The challenge item inventory.
/// </summary>
public sealed class ChallengeItemInventory
{
    /// <summary>
    /// The slots of the inventory
    /// </summary>
    public ChallengeItemSlot[,] Slots { get; private set; }

    public int CurrentWidth { get; private set; }

    public int CurrentHeight { get; private set; }

    public int MaxWidth { get; private set; }

    public int MaxHeight { get; private set; }

    public ChallengeItemInventory(int initialWidth, int initialHeight, int maxWidth, int maxHeight)
    {
        Slots = new ChallengeItemSlot[initialWidth, initialHeight];
        CurrentWidth = initialWidth;
        CurrentHeight = initialHeight;
        MaxWidth = maxWidth;
        MaxHeight = maxHeight;
    }

    /// <summary>
    /// Upgrades the inventory size.
    /// </summary>
    /// <param name="newSize"></param>
    /// <returns></returns>
    public bool Upgrade(int newWidth, int newHeight)
    {
        if (newWidth > MaxWidth || newHeight > MaxHeight || newWidth <= CurrentWidth || newHeight <= CurrentHeight)
        {
            // Do not upgrade if the new dimensions are greater than the maximum dimensions or less than or equal to the current dimensions
            return false;
        }

        // Create a new array with the new dimensions
        var newSlots = new ChallengeItemSlot[newWidth, newHeight];

        // Copy the contents of the old array into the new array
        for (int i = 0; i < CurrentWidth; i++)
        {
            for (int j = 0; j < CurrentHeight; j++)
            {
                newSlots[i, j] = Slots[i, j];
            }
        }

        // Replace the old array with the new array
        Slots = newSlots;
        CurrentWidth = newWidth;
        CurrentHeight = newHeight;

        return true;
    }
}
