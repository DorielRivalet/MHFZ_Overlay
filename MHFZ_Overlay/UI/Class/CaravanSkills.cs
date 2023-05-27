// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;

namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class CaravanSkills
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public long CaravanSkillsID { get; set; }
        public long RunID { get; set; }
        public long CaravanSkill1ID { get; set; }
        public long CaravanSkill2ID { get; set; }
        public long CaravanSkill3ID { get; set; }

    }
}
