// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

// TODO: ORM
public sealed class GearCompendium
{
    public long MostUsedWeaponType { get; set; }

    public long TotalUniqueArmorPiecesUsed { get; set; }

    public long TotalUniqueWeaponsUsed { get; set; }

    public long TotalUniqueDecorationsUsed { get; set; }

    public long MostCommonDecorationID { get; set; }

    public long LeastUsedArmorSkill { get; set; }
}
