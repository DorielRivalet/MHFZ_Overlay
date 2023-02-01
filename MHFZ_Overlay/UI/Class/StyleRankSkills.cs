using System;

namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class StyleRankSkills
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public long StyleRankSkillsID { get; set; }
        public long RunID { get; set; }
        public long StyleRankSkill1ID { get; set; }
        public long StyleRankSkill2ID { get; set; }
    }
}
