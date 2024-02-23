// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using MHFZ_Overlay.Models.Structures;

// TODO: ORM
public sealed class DivaPrayerGemPoint
{
    public DivaPrayerGemColor Color { get; set; } = DivaPrayerGemColor.None;

    public int Points { get; set; }

    public int MaxUses { get; set; }

    public int Level { get; set; }
}
