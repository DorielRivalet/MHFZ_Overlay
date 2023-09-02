// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The bingo difficulty carts base.
/// </summary>
public static class BingoDifficultyCarts
{
    public static ReadOnlyDictionary<Difficulty, int> DifficultyCarts { get; } = new(new Dictionary<Difficulty, int>
        {
            {
                Difficulty.Easy, 7
            },
            {
                Difficulty.Medium, 5
            },
            {
                Difficulty.Hard, 3
            },
            {
                Difficulty.Extreme, 0
            },
        });
}
