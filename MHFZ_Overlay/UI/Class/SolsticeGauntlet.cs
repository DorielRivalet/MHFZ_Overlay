// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;

namespace MHFZ_Overlay.UI.Class;

public class SolsticeGauntlet
{
    public long SolsticeGauntletID { get; set; }
    public string WeaponType { get; set; } = "Any";
    public string Category { get; set; } = "Standard";
    public long TotalFramesElapsed { get; set; }
    public string TotalTimeElapsed { get; set; } = DateTime.MaxValue.ToString();
    public long Run1ID { get; set; }
    public long Run2ID { get; set; }
    public long Run3ID { get; set; }
    public long Run4ID { get; set; }
    public long Run5ID { get; set; }
    public long Run6ID { get; set; }

}
