// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;   

/// <summary>
/// The challenge items list.
/// </summary>
public static class ChallengeItems
{
    public static ReadOnlyDictionary<int, ChallengeItem> IDChallengeItem { get; } = new(new Dictionary<int, ChallengeItem>
    {
        {
            0, new ChallengeItem
            {
                Name = "None",
                Description = string.Empty,
                ImageLink = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
                IsStackable = false,
                MaxStackSize = 1,
                IsUnique = false,
                Rarity = 1,
            }
        },
    });
}
