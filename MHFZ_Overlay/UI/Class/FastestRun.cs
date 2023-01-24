using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MHFZ_Overlay.UI.Class
{
    public class FastestRun
    {
        public string ObjectiveImage { get; set; }
        public string QuestName { get; set; }
        public long RunID { get; set; }
        public long QuestID { get; set; }
        public string YoutubeID { get; set; }
        public string FinalTimeDisplay { get; set; }
        public DateTime Date { get; set; }
        public string ActualOverlayMode { get; set; }
        public long PartySize { get; set; }
    }
}
