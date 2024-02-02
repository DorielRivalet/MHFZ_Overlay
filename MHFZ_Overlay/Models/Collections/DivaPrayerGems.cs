// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The diva prayer gems list.
/// </summary>
public static class DivaPrayerGems
{
    public static ReadOnlyDictionary<DivaPrayerGemType, DivaPrayerGem> PrayerGems { get; } = new(new Dictionary<DivaPrayerGemType, DivaPrayerGem>
    {
        { DivaPrayerGemType.None, new DivaPrayerGem(){ Description= "None", Level=0, MaxLevel=0, Type = DivaPrayerGemType.None} },
        { DivaPrayerGemType.WindStorm, new DivaPrayerGem(){ Description= "Sharpness does not decrease with blademaster weapons. Works for 5, 10 or 20 quests depending on level during the prayer active window.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.WindStorm} },
        { DivaPrayerGemType.Agility, new DivaPrayerGem(){ Description= "Reduces the recoil and reload speed of Gunner weapons.", Level=0, MaxLevel=7, Type = DivaPrayerGemType.Agility, PartyEffect=true, Unused=true} },
        { DivaPrayerGemType.SeveringPower, new DivaPrayerGem(){ Description= "Tails can be cut with any damage type.", Level=0, MaxLevel=1, Type = DivaPrayerGemType.SeveringPower} },
        { DivaPrayerGemType.Elegance, new DivaPrayerGem(){ Description= "Adds passive HP recovery to all quests.", Level=0, MaxLevel=2, Type = DivaPrayerGemType.Elegance} },
        { DivaPrayerGemType.Earth, new DivaPrayerGem(){ Description= "Makes it easier to scare monsters by attacking with Earth Style.", Level=0, MaxLevel=4, Type = DivaPrayerGemType.Earth, Unused=true} },
        { DivaPrayerGemType.Heaven, new DivaPrayerGem(){ Description= "Makes it easier to scare monsters by attacking with Heaven Style.", Level=0, MaxLevel=4, Type = DivaPrayerGemType.Heaven, Unused=true} },
        { DivaPrayerGemType.Tempest, new DivaPrayerGem(){ Description= "Makes it easier to scare monsters by attacking with Storm Style.", Level=0, MaxLevel=4, Type = DivaPrayerGemType.Tempest, Unused=true} },
        { DivaPrayerGemType.CuttingEdge, new DivaPrayerGem(){ Description= "Increases the amount of raw damage dealt by a cutting weapon by adjusting hitboxes to be weaker against the damage type.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.CuttingEdge} },
        { DivaPrayerGemType.Striking, new DivaPrayerGem(){ Description= "Increases the amount of raw damage dealt by an impact weapon by adjusting hitboxes to be weaker against the damage type.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Striking} },
        { DivaPrayerGemType.RisingBullet, new DivaPrayerGem(){ Description= "Increases the amount of raw damage dealt by a ranged weapon by adjusting hitboxes to be weaker against the damage type.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.RisingBullet} },
        { DivaPrayerGemType.StatusLength, new DivaPrayerGem(){ Description= "Increases the duration of status effects on monsters.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.StatusLength, PartyEffect=true} },
        { DivaPrayerGemType.Abnormality, new DivaPrayerGem(){ Description= "Monsters are more susceptible to status ailments.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Abnormality, PartyEffect=true, Unused=true} },
        { DivaPrayerGemType.Lethality, new DivaPrayerGem(){ Description= "Increases damage when striking body parts upon which your attacks are highly effective. However, element damage does not change.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Lethality, Unused=true} },
        { DivaPrayerGemType.HeavyThunder, new DivaPrayerGem(){ Description= "Elemental damage increases based on level.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.HeavyThunder} },
        { DivaPrayerGemType.Unshakable, new DivaPrayerGem(){ Description= "Monsters cannot flee if in the same area as a hunter.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Unshakable, PartyEffect=true} },
        { DivaPrayerGemType.Ringing, new DivaPrayerGem(){ Description= "Adds new items to the GCP store based on level.", Level=0, MaxLevel=1, Type = DivaPrayerGemType.Ringing} },
        { DivaPrayerGemType.Mobilisation, new DivaPrayerGem(){ Description= "Attack will go up based on the number of human hunters in a quest.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Mobilisation, PartyEffect=true} },
        { DivaPrayerGemType.Protection, new DivaPrayerGem(){ Description= "Gives Divine Protection, Goddess' Embrace or Soul Revival based on level.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Protection} },
        { DivaPrayerGemType.PowerfulStrikes, new DivaPrayerGem(){ Description= "Increases affinity of all weapons based on the level of the song.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.PowerfulStrikes} },
        { DivaPrayerGemType.Fireproof, new DivaPrayerGem(){ Description= "Increases fire resistance.", Level=0, MaxLevel=0, Type = DivaPrayerGemType.Fireproof, Unused=true} },
        { DivaPrayerGemType.Waterproof, new DivaPrayerGem(){ Description= "Increases water resistance.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Waterproof, Unused=true} },
        { DivaPrayerGemType.Iceproof, new DivaPrayerGem(){ Description= "Increases ice resistance.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Iceproof, Unused=true} },
        { DivaPrayerGemType.Dragonproof, new DivaPrayerGem(){ Description= "Increases dragon resistance.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Dragonproof, Unused=true} },
        { DivaPrayerGemType.Thunderproof, new DivaPrayerGem(){ Description= "Increases thunder resistance.", Level=0, MaxLevel=3, Type = DivaPrayerGemType.Thunderproof, Unused=true} },
        { DivaPrayerGemType.Immunity, new DivaPrayerGem(){ Description= "Increases resistance to all elements.", Level=0, MaxLevel=2, Type = DivaPrayerGemType.Immunity, Unused=true} },

    });
}
