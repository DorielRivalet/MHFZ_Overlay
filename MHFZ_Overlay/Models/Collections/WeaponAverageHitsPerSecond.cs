// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// The weapon average hits per second list.
/// </summary>
public static class WeaponAverageHitsPerSecond
{
    public static ReadOnlyDictionary<int, double> WeaponAverageHitsPerSecondID { get; } = new (new Dictionary<int, double>
    {
        { 0, 0.3 },
        { 1, 2.25 },
        { 2, 0.75 },
        { 3, 1.95 },
        { 4, 2.65 },
        { 5, 3.65 },
        { 6, 4 },
        { 7, 1.75 },
        { 8, 1.6 },
        { 9, 3 },
        { 10, 1.3 },
        { 11, 1.95 },
        { 12, 1.5 },
        { 13, 1.6 },
    });
}
