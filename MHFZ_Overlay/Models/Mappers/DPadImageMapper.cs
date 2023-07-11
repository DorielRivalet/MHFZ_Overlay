// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Mappers;

using System.Collections.Generic;
using MHFZ_Overlay.Models.Structures;

public static class DPadImageMapper
{
    public static IReadOnlyDictionary<Direction, string> imagePaths = new Dictionary<Direction, string>
    {
        { Direction.None, "Assets/Icons/png/gamepad_dpad.png" },
        { Direction.Up, "Assets/Icons/png/gamepad_dpad_up.png" },
        { Direction.UpRight, "Assets/Icons/png/gamepad_dpad_upright.png" },
        { Direction.Right, "Assets/Icons/png/gamepad_dpad_right.png" },
        { Direction.DownRight, "Assets/Icons/png/gamepad_dpad_downright.png" },
        { Direction.Down, "Assets/Icons/png/gamepad_dpad_down.png" },
        { Direction.DownLeft, "Assets/Icons/png/gamepad_dpad_downleft.png" },
        { Direction.Left, "Assets/Icons/png/gamepad_dpad_left.png" },
        { Direction.UpLeft, "Assets/Icons/png/gamepad_dpad_upleft.png" },
    };

    public static string GetImage(Direction direction)
    {
        if (imagePaths.TryGetValue(direction, out var imagePath))
        {
            return imagePath;
        }

        // Return a default image path or handle the case where direction is not found
        return "Assets/Icons/png/gamepad_dpad.png";
    }
}
