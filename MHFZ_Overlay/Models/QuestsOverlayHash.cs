// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class QuestsOverlayHash
{
    public long? QuestsOverlayHashID { get; set; }

    public string? OverlayHash { get; set; }

    public long? RunID { get; set; }
}
