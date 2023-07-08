// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Mappers;

using System.Collections.Generic;
using MHFZ_Overlay.Models.Structures;

public static class JoystickImageMapper
{
    public static IReadOnlyDictionary<Direction, string> imagePaths = new Dictionary<Direction, string>
    {
        { Direction.None, "Assets/Icons/png/gamepad_joystick.png" },
        { Direction.Up, "Assets/Icons/png/gamepad_joystick_up.png" },
        { Direction.UpRight, "Assets/Icons/png/gamepad_joystick_upright.png" },
        { Direction.Right, "Assets/Icons/png/gamepad_joystick_right.png" },
        { Direction.DownRight, "Assets/Icons/png/gamepad_joystick_downright.png" },
        { Direction.Down, "Assets/Icons/png/gamepad_joystick_down.png" },
        { Direction.DownLeft, "Assets/Icons/png/gamepad_joystick_downleft.png" },
        { Direction.Left, "Assets/Icons/png/gamepad_joystick_left.png" },
        { Direction.UpLeft, "Assets/Icons/png/gamepad_joystick_upleft.png" }
    };

    public static string GetImage(Direction direction)
    {
        if (imagePaths.TryGetValue(direction, out var imagePath))
        {
            return imagePath;
        }

        // Return a default image path or handle the case where direction is not found
        return "Assets/Icons/png/gamepad_joystick.png";
    }
}
