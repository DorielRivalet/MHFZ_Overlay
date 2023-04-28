using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFZ_Overlay.UI.Class
{
    // Create a class to store ItemIDs for each row in PlayerInventory
    public class PlayerInventoryItemIds
    {
        public long PlayerInventoryID { get; set; }
        public List<long> ItemIds { get; set; }
    }

}
