// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class CaravanSkills
{
    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public long CaravanSkillsID { get; set; }

    public long RunID { get; set; }

    public long CaravanSkill1ID { get; set; }

    public long CaravanSkill2ID { get; set; }

    public long CaravanSkill3ID { get; set; }
}
