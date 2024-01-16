// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

/// <summary>
/// Commonly, the frames each monster 1 hp percent took for every 10% remaining hp.
/// </summary>
public sealed class QuestSplit
{
    public int? ZeroPercentRemainingHPFrames { get; set; }
    public int? TenPercentRemainingHPFrames { get; set; }
    public int? TwentyPercentRemainingHPFrames { get; set; }
    public int? ThirtyPercentRemainingHPFrames { get; set; }
    public int? FortyPercentRemainingHPFrames { get; set; }
    public int? FiftyPercentRemainingHPFrames { get; set; }
    public int? SixtyPercentRemainingHPFrames { get; set; }
    public int? SeventyPercentRemainingHPFrames { get; set; }
    public int? EightyPercentRemainingHPFrames { get; set; }
    public int? NinetyPercentRemainingHPFrames { get; set; }
    public int? Sum() => this.NinetyPercentRemainingHPFrames + this.EightyPercentRemainingHPFrames + this.SeventyPercentRemainingHPFrames + this.SixtyPercentRemainingHPFrames + this.FiftyPercentRemainingHPFrames + this.FortyPercentRemainingHPFrames + this.ThirtyPercentRemainingHPFrames + this.TwentyPercentRemainingHPFrames + this.TenPercentRemainingHPFrames + this.ZeroPercentRemainingHPFrames;
}
