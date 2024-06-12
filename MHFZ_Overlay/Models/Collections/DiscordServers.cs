// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// The discord servers list.
/// </summary>
public static class DiscordServers
{
    public static ReadOnlyDictionary<long, DiscordServer> DiscordServerInfo { get; } = new (new Dictionary<long, DiscordServer>
    {
        { 0, new DiscordServer {Name = "Local", ID=0, InviteLink="" } },
        { 1, new DiscordServer {Name = "Monster Hunter Frontier: Renewal", ID=932246672392740917, InviteLink="https://discord.gg/U4pjzmzUVx" } },
        { 2, new DiscordServer {Name = "Rain Frontier Server", ID=937230168223789066, InviteLink="https://discord.gg/TcpkpUpeGw" } },
        { 3, new DiscordServer {Name = "MHF-Z Server Ancient", ID=759022449743495170, InviteLink="https://discord.gg/csR8RPg" } },
        { 4, new DiscordServer {Name = "MezeLounge", ID=973963573619486740, InviteLink="https://discord.gg/uE3rZMSbnZ" } },
        //{ 5, new DiscordServer {Name = "Hunsterverse", ID=288170871908990976, InviteLink="" } }, // they dont accept random invites
        { 6, new DiscordServer {Name = "Arca", ID=967058504403808356, InviteLink="https://discord.gg/MCYGFWHcDK" } },
    });
}
