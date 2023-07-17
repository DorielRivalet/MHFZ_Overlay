// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

///<summary>
///The discord servers list
///</summary>
public static class DiscordServersCollection
{
    public static ReadOnlyDictionary<long, string> DiscordServerID { get; } = new (new Dictionary<long, string>
    {
        { 0, "Local" },
        { 932246672392740917, "Monster Hunter Frontier: Renewal" },
        { 937230168223789066, "Rain Frontier Server" },
        { 759022449743495170, "Monster Hunter [Ancient_Warriors]" },
        { 973963573619486740, "MezeLounge" },
        { 288170871908990976, "Hunsterverse" },
        { 967058504403808356, "Arca" },
    });
}
