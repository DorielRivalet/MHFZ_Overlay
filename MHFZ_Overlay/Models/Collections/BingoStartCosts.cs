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
/// The bingo start base costs.
/// </summary>
public static class BingoStartCosts
{
    public static ReadOnlyDictionary<Difficulty, int> DifficultyCosts { get; } = new(new Dictionary<Difficulty, int>
        {
            {
                Difficulty.Easy, 0
            },
            {
                Difficulty.Medium, 50
            },
            {
                Difficulty.Hard, 250
            },
            {
                Difficulty.Extreme, 1000
            },
        });

    public static ReadOnlyDictionary<GauntletBoost, int> GauntletBoostCosts { get; } = new (new Dictionary<GauntletBoost, int>
        {
            {
                GauntletBoost.None, 0
            },
            {
                GauntletBoost.Zenith, 10
            },
            {
                GauntletBoost.Solstice, 20
            },
            {
                GauntletBoost.Musou, 30
            },
        });

    public static int MusouElzelionBoostCost = 100;
}
