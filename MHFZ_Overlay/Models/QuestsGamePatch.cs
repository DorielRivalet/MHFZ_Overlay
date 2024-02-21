// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class QuestsGamePatch
{
    public long? QuestsGamePatchID { get; set; }

    public long? RunID { get; set; }

    public string? mhfdatInfo { get; set; }

    public string? mhfemdInfo { get; set; }

    public string? mhfodllInfo { get; set; }

    public string? mhfohddllInfo { get; set; }
}
