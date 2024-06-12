// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class DiscordServer
{
    public string Name { get; set; } = string.Empty;

    public long ID { get; set; }

    public string InviteLink { get; set; } = string.Empty;
}
