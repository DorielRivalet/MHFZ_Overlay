// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// The diva prayer gems points target list.
/// </summary>
public static class DivaPrayerGemsPoints
{
    public static ReadOnlyDictionary<int, DivaPrayerGemPoint> Targets { get; } = new (new Dictionary<int, DivaPrayerGemPoint>
    {
        { 0, new DivaPrayerGemPoint(){ Points = 0, MaxUses = 0, Level = 0,} },
        { 500000, new DivaPrayerGemPoint(){ Points = 500_000, MaxUses = 1, Level = 0,} },
        { 1000000, new DivaPrayerGemPoint(){ Points = 1_000000, MaxUses = 2, Level = 0,} },
        { 2000000, new DivaPrayerGemPoint(){ Points = 2_000000, MaxUses = 3, Level = 0,} },
        { 3000000, new DivaPrayerGemPoint(){ Points = 3_000000, MaxUses = 4, Level = 0,} },
        { 4000000, new DivaPrayerGemPoint(){ Points = 4_000000, MaxUses = 5, Level = 0,} },
        { 5000000, new DivaPrayerGemPoint(){ Points = 5_000000, MaxUses = 6, Level = 0,} },
        { 6000000, new DivaPrayerGemPoint(){ Points = 6_000000, MaxUses = 7, Level = 0,} },
        { 7000000, new DivaPrayerGemPoint(){ Points = 7_000000, MaxUses = 8, Level = 0,} },
        { 8000000, new DivaPrayerGemPoint(){ Points = 8_000000, MaxUses = 9, Level = 0,} },
        { 9000000, new DivaPrayerGemPoint(){ Points = 9_000_000, MaxUses = 10, Level = 1,} },

        { 10000000, new DivaPrayerGemPoint(){ Points = 10_000000, MaxUses = 11, Level = 1,} },
        { 15000000, new DivaPrayerGemPoint(){ Points = 15_000000, MaxUses = 12, Level = 1,} },
        { 20000000, new DivaPrayerGemPoint(){ Points = 20_000000, MaxUses = 13, Level = 1,} },
        { 25000000, new DivaPrayerGemPoint(){ Points = 25_000000, MaxUses = 14, Level = 1,} },
        { 30000000, new DivaPrayerGemPoint(){ Points = 30_000000, MaxUses = 15, Level = 2,} },

        { 35000000, new DivaPrayerGemPoint(){ Points = 35_000000, MaxUses = 16, Level = 2,} },
        { 40000000, new DivaPrayerGemPoint(){ Points = 40_000000, MaxUses = 17, Level = 2,} },
        { 45000000, new DivaPrayerGemPoint(){ Points = 45_000000, MaxUses = 18, Level = 2,} },
        { 50000000, new DivaPrayerGemPoint(){ Points = 50_000000, MaxUses = 19, Level = 2,} },
        { 55000000, new DivaPrayerGemPoint(){ Points = 55_000000, MaxUses = 20, Level = 3,} },

        { 60000000, new DivaPrayerGemPoint(){ Points = 60_000_000, MaxUses = 21, Level = 3,} },
        { 70000000, new DivaPrayerGemPoint(){ Points = 70_000_000, MaxUses = 22, Level = 3,} },
        { 80000000, new DivaPrayerGemPoint(){ Points = 80_000_000, MaxUses = 23, Level = 3,} },
        { 90000000, new DivaPrayerGemPoint(){ Points = 90_000_000, MaxUses = 24, Level = 3,} },
        { 100000000, new DivaPrayerGemPoint(){ Points = 100_000_000, MaxUses = 25, Level = 3,} },
    });
}
