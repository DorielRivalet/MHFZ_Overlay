// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

public sealed class QuestAttempts
{
    public long QuestAttemptsID { get; set; }

    public long QuestID { get; set; }

    public long WeaponTypeID { get; set; }

    public string ActualOverlayMode { get; set; } = string.Empty;

    public long Attempts { get; set; }

    public long RunBuffs { get; set; }

}
