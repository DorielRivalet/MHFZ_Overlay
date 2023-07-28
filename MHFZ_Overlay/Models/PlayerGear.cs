// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class PlayerGear
{
    public string PlayerGearHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public long PlayerGearID { get; set; }

    public long RunID { get; set; }

    public long PlayerID { get; set; }

    public string GearName { get; set; } = string.Empty;

    public long StyleID { get; set; }

    public long WeaponIconID { get; set; }

    public long WeaponClassID { get; set; }

    public long WeaponTypeID { get; set; }

    public long? BlademasterWeaponID { get; set; }

    public long? GunnerWeaponID { get; set; }

    public string WeaponSlot1 { get; set; } = string.Empty;

    public string WeaponSlot2 { get; set; } = string.Empty;

    public string WeaponSlot3 { get; set; } = string.Empty;

    public long HeadID { get; set; }

    public long HeadSlot1ID { get; set; }

    public long HeadSlot2ID { get; set; }

    public long HeadSlot3ID { get; set; }

    public long ChestID { get; set; }

    public long ChestSlot1ID { get; set; }

    public long ChestSlot2ID { get; set; }

    public long ChestSlot3ID { get; set; }

    public long ArmsID { get; set; }

    public long ArmsSlot1ID { get; set; }

    public long ArmsSlot2ID { get; set; }

    public long ArmsSlot3ID { get; set; }

    public long WaistID { get; set; }

    public long WaistSlot1ID { get; set; }

    public long WaistSlot2ID { get; set; }

    public long WaistSlot3ID { get; set; }

    public long LegsID { get; set; }

    public long LegsSlot1ID { get; set; }

    public long LegsSlot2ID { get; set; }

    public long LegsSlot3ID { get; set; }

    public long Cuff1ID { get; set; }

    public long Cuff2ID { get; set; }

    public long ZenithSkillsID { get; set; }

    public long AutomaticSkillsID { get; set; }

    public long ActiveSkillsID { get; set; }

    public long CaravanSkillsID { get; set; }

    public long DivaSkillID { get; set; }

    public long GuildFoodID { get; set; }

    public long StyleRankSkillsID { get; set; }

    public long PlayerInventoryID { get; set; }

    public long AmmoPouchID { get; set; }

    public long PartnyaBagID { get; set; }

    public long PoogieItemID { get; set; }

    public long RoadDureSkillsID { get; set; }

    // TODO idk how to show these
    public string? PlayerInventoryDictionary { get; set; } = string.Empty;

    public string? PlayerAmmoPouchDictionary { get; set; } = string.Empty;

    public string? PartnyaBagDictionary { get; set; } = string.Empty;
}
