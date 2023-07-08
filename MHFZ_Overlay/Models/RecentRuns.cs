// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using MHFZ_Overlay.Models.Constant;

public class RecentRuns
{
    public string ObjectiveImage { get; set; } = Messages.MONSTER_IMAGE_NOT_LOADED;

    public string QuestName { get; set; } = string.Empty;

    public long RunID { get; set; }

    public long QuestID { get; set; }

    public string YoutubeID { get; set; } = Messages.RICK_ROLL_ID;

    public string FinalTimeDisplay { get; set; } = Messages.MAXIMUM_TIMER_PLACEHOLDER;

    public DateTime Date { get; set; }

    public string ActualOverlayMode { get; set; } = Messages.OVERLAY_MODE_PLACEHOLDER;

    public long PartySize { get; set; }
}
