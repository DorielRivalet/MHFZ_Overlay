// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class WeaponCanUseReflect
{
    public static ReadOnlyDictionary<int, bool> WeaponTypeID { get; } = new (new Dictionary<int, bool>
    {
        { 0, true },
        { 1, false },
        { 2, false },
        { 3, true },
        { 4, true },
        { 5, false },
        { 6, false },
        { 7, true },
        { 8, false },
        { 9, true },
        { 10, false },
        { 11, true },
        { 12, true },
        { 13, true },
    });
}
