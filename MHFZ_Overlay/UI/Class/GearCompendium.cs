// Copyright 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class GearCompendium
    {
        public long MostUsedWeaponType { get; set; }
        public long TotalUniqueArmorPiecesUsed { get; set; }
        public long TotalUniqueWeaponsUsed { get; set; }
        public long TotalUniqueDecorationsUsed { get; set; }
        public long MostCommonDecorationID { get; set; }
        public long LeastUsedArmorSkill { get; set; }
    }
}
