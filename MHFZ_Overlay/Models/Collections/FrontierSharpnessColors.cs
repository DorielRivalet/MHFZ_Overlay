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
/// The colors depending on sharpness.
/// </summary>
public static class FrontierSharpnessColors
{
    public static ReadOnlyDictionary<FrontierSharpness, string> SharpnessColors { get; } = new(new Dictionary<FrontierSharpness, string>
        {
            {
                FrontierSharpness.Red, "#c50f3a"
            },
            {
                FrontierSharpness.Orange, "#e85218"
            },
            {
                FrontierSharpness.Yellow, "#f3c832"
            },
            {
                FrontierSharpness.Green, "#5ed300"
            },
            {
                FrontierSharpness.Blue, "#3068ee"
            },
            {
                FrontierSharpness.White, "#f0f0f0"
            },
            {
                FrontierSharpness.Purple, "#de7aff"
            },
            {
                FrontierSharpness.Cyan, "#86f4f4"
            },
        });
}
