// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

public sealed class ButtonPress
{
    public ButtonPress(string buttonType, int row, int column, string icon, object content)
    {
        this.ButtonType = buttonType;
        this.Row = row;
        this.Column = column;
        this.Icon = icon;
        this.Content = content;
    }

    public string ButtonType { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }

    public string Icon { get; set; }

    public object Content { get; set; }
}
