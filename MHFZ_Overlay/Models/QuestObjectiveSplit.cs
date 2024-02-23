// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

/// <summary>
/// Commonly, the frames each monster 1 hp percent took for every quest objective.
/// </summary>
public sealed class QuestObjectiveSplit
{
    public int? ZeroPercentRemainingHPFrames { get; set; }
    
    public int? TwentyPercentRemainingHPFrames { get; set; }
   
    public int? FortyPercentRemainingHPFrames { get; set; }

    public int? Sum() => this.FortyPercentRemainingHPFrames + this.TwentyPercentRemainingHPFrames + this.ZeroPercentRemainingHPFrames;
}
