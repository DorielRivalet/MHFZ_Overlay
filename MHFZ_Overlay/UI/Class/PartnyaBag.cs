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
    public class PartnyaBag
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public long PartnyaBagID { get; set; }
        public long RunID { get; set; }
        public long Item1ID { get; set; }
        public long Item1Quantity { get; set; }

        public long Item2ID { get; set; }
        public long Item2Quantity { get; set; }
        public long Item3ID { get; set; }
        public long Item3Quantity { get; set; }

        public long Item4ID { get; set; }
        public long Item4Quantity { get; set; }

        public long Item5ID { get; set; }
        public long Item5Quantity { get; set; }

        public long Item6ID { get; set; }
        public long Item6Quantity { get; set; }

        public long Item7ID { get; set; }
        public long Item7Quantity { get; set; }

        public long Item8ID { get; set; }
        public long Item8Quantity { get; set; }

        public long Item9ID { get; set; }
        public long Item9Quantity { get; set; }

        public long Item10ID { get; set; }
        public long Item10Quantity { get; set; }

    }
}
