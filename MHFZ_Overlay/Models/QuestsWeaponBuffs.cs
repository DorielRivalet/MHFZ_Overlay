// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;

// TODO: ORM
public sealed class QuestsWeaponBuffs
{
    public long? QuestsWeaponBuffsID { get; set; }

    public Dictionary<long, long> DualSwordsSharpensDictionary { get; set; } = new Dictionary<long, long>();

    public long? RunID { get; set; }
}
