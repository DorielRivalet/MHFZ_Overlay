using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class AutomaticSkills
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public long AutomaticSkillsID { get; set; }
        public long RunID { get; set; }
        public long AutomaticSkill1ID { get; set; }
        public long AutomaticSkill2ID { get; set; }
        public long AutomaticSkill3ID { get; set; }
        public long AutomaticSkill4ID { get; set; }
        public long AutomaticSkill5ID { get; set; }
        public long AutomaticSkill6ID { get; set; }

    }
}
