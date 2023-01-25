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
