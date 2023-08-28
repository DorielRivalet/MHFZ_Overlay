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
/// The colors depending on item rarity.
/// </summary>
public static class FrontierRarityColors
{
    public static ReadOnlyDictionary<int, string> RarityColors { get; } = new(new Dictionary<int, string>
        {
            {
                1, "#efefe9"
            },
            {
                2, "#efefe9"
            },
            {
                3, "#efefe9"
            },
            {
                4, "#73cb8d"
            },
            {
                5, "#ed93a4"
            },
            {
                6, "#96b5fd"
            },
            {
                7, "#ff985d"
            },
            {
                8, "#fffd2e"
            },
            {
                9, "#c8ff6a"
            },
            {
                10, "#68ecec"
            },
            {
                11, "#cba6fa"
            },
            {
                12, "#ff435d"
            },
        });
}
