// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using MHFZ_Overlay.Models.Structures;

// TODO: ORM
public sealed class DivaPrayerGem
{
    public DivaPrayerGemType Type { get; set; }

    public int Level { get; set; }

    public int MaxLevel { get; set; }

    public string Description { get; set; } = "None";

    /// <summary>
    /// Unused in official servers.
    /// </summary>
    public bool Unused { get; set; }

    /// <summary>
    /// Whether the gem affects the whole party or only the user.
    /// </summary>
    public bool PartyEffect { get; set; }
}
