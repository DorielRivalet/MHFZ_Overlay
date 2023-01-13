using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFZ_Overlay.UI.Class
{
    public class ButtonPress
    {
        public string ButtonType { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Icon { get; set; }

        public object Content { get; set; }

        public ButtonPress(string buttonType, int row, int column, string icon, object content)
        {
            ButtonType = buttonType;
            Row = row;
            Column = column;
            Icon = icon;
            Content = content;
        }
    }
}
