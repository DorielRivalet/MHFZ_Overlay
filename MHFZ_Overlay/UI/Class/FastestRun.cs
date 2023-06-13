// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay.Core.Constant;
using System;

namespace MHFZ_Overlay.UI.Class;

public class FastestRun
{
    public string ObjectiveImage { get; set; } = Messages.MONSTER_IMAGE_NOT_LOADED;
    public string QuestName { get; set; } = string.Empty;
    public long RunID { get; set; }
    public long QuestID { get; set; }
    public string YoutubeID { get; set; } = Messages.RICK_ROLL_ID;
    public string FinalTimeDisplay { get; set; } = Messages.MAXIMUM_TIMER_PLACEHOLDER;
    public DateTime Date { get; set; }
}
