// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// The skill fruits list.
/// </summary>
public static class SkillFruits
{
    public static ReadOnlyDictionary<long, bool> ItemID { get; } = new (new Dictionary<long, bool>
    {
        { 4745, true },
        { 4746, true },
        { 4747, true },
        { 4748, true },
        { 4749, true },
        { 5122, true },
        { 5123, true },
        { 5124, true },
        { 5125, true },
        { 5126, true },
        { 5795, true },
        { 5796, true },
        { 5797, true },
        { 5798, true },
        { 5799, true },
        { 6168, true },
        { 6169, true },
        { 6170, true },
        { 6171, true },
        { 6172, true },
        { 8026, true },
        { 8027, true },
        { 8028, true },
        { 8029, true },
        { 8030, true },
    });
}
