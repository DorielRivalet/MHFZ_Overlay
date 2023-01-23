using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFZ_Overlay.UI.Class.Mapper
{
    public class WeaponUsageMapper
    {
        public string WeaponType { get; set; }
        public string Style { get; set; }
        public long RunCount { get; set; }

        public WeaponUsageMapper(string weaponType, string style, int runCount)
        {
            this.WeaponType = weaponType;
            this.Style = style;
            this.RunCount = runCount;
        }
    }
}
