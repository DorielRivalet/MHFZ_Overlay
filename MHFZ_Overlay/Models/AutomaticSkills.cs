// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class AutomaticSkills
{
    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public long AutomaticSkillsID { get; set; }

    public long RunID { get; set; }

    public long AutomaticSkill1ID { get; set; }

    public long AutomaticSkill2ID { get; set; }

    public long AutomaticSkill3ID { get; set; }

    public long AutomaticSkill4ID { get; set; }

    public long AutomaticSkill5ID { get; set; }

    public long AutomaticSkill6ID { get; set; }
}
