// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Globalization;

public sealed class SolsticeGauntlet
{
    public long SolsticeGauntletID { get; set; }

    public string WeaponType { get; set; } = "Any";

    public string Category { get; set; } = "Standard";

    public long TotalFramesElapsed { get; set; }

    public string TotalTimeElapsed { get; set; } = DateTime.MaxValue.ToString(CultureInfo.InvariantCulture);

    public long Run1ID { get; set; }

    public long Run2ID { get; set; }

    public long Run3ID { get; set; }

    public long Run4ID { get; set; }

    public long Run5ID { get; set; }

    public long Run6ID { get; set; }
}
