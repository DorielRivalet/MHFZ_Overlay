// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

public sealed class WeaponUsage
{
    public WeaponUsage(string weaponType, string style, int runCount)
    {
        this.WeaponType = weaponType;
        this.Style = style;
        this.RunCount = runCount;
    }

    public string WeaponType { get; set; }

    public string Style { get; set; }

    public long RunCount { get; set; }
}
