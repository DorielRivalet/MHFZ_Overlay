// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using MHFZ_Overlay.Models.Constant;

public sealed class RecentRuns
{
    public string ObjectiveImage { get; set; } = Messages.MonsterImageNotLoaded;

    public string QuestName { get; set; } = string.Empty;

    public long RunID { get; set; }

    public long QuestID { get; set; }

    public string YouTubeID { get; set; } = Messages.RickRollID;

    public string FinalTimeDisplay { get; set; } = Messages.MaximumTimerPlaceholder;

    public DateTime Date { get; set; }

    public string ActualOverlayMode { get; set; } = Messages.OverlayModePlaceholder;

    public long PartySize { get; set; }

    public long RunBuffs { get; set; }

}
