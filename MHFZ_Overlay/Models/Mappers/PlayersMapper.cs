// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Mappers;

using System;
using System.Collections.Generic;
using System.Globalization;

///<summary>
///The players list
///</summary>
public static class PlayersList
{
    public static IReadOnlyDictionary<int, List<string>> PlayerIDs { get; } = new Dictionary<int, List<string>>
    {
        // No Player
        { 0, new List<string> { DateTime.UnixEpoch.Date.ToString(CultureInfo.InvariantCulture), "None", "NoGuild", "0", "Unknown", "Japan" } },

        // Local Player
        { 1, new List<string> { DateTime.UtcNow.Date.ToString(CultureInfo.InvariantCulture), "HunterName", "GuildName","0", "Unknown", "Japan" } },
    };
}
