// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class ZenithSkills
{
    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public long ZenithSkillsID { get; set; }

    public long RunID { get; set; }

    public long ZenithSkill1ID { get; set; }

    public long ZenithSkill2ID { get; set; }

    public long ZenithSkill3ID { get; set; }

    public long ZenithSkill4ID { get; set; }

    public long ZenithSkill5ID { get; set; }

    public long ZenithSkill6ID { get; set; }

    public long ZenithSkill7ID { get; set; }
}
