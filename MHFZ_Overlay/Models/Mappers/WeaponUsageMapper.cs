// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Mappers;

public class WeaponUsageMapper
{
    public string WeaponType { get; set; }

    public string Style { get; set; }

    public long RunCount { get; set; }

    public WeaponUsageMapper(string weaponType, string style, int runCount)
    {
        this.WeaponType = weaponType;
        this.Style = style;
        this.RunCount = runCount;
    }
}
