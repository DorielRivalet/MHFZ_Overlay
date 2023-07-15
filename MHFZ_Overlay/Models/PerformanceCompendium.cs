// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

// TODO: ORM
public sealed class PerformanceCompendium
{
    public double HighestTrueRaw { get; set; }

    public double TrueRawAverage { get; set; }

    public double TrueRawMedian { get; set; }

    public long HighestTrueRawRunID { get; set; }

    public double HighestSingleHitDamage { get; set; }

    public double SingleHitDamageAverage { get; set; }

    public double SingleHitDamageMedian { get; set; }

    public long HighestSingleHitDamageRunID { get; set; }

    public double HighestHitCount { get; set; }

    public double HitCountAverage { get; set; }

    public double HitCountMedian { get; set; }

    public long HighestHitCountRunID { get; set; }

    public double HighestHitsTakenBlocked { get; set; }

    public double HitsTakenBlockedAverage { get; set; }

    public double HitsTakenBlockedMedian { get; set; }

    public long HighestHitsTakenBlockedRunID { get; set; }

    public double HighestDPS { get; set; }

    public double DPSAverage { get; set; }

    public double DPSMedian { get; set; }

    public long HighestDPSRunID { get; set; }

    public double HighestHitsPerSecond { get; set; }

    public double HitsPerSecondAverage { get; set; }

    public double HitsPerSecondMedian { get; set; }

    public long HighestHitsPerSecondRunID { get; set; }

    public double HighestHitsTakenBlockedPerSecond { get; set; }

    public double HitsTakenBlockedPerSecondAverage { get; set; }

    public double HitsTakenBlockedPerSecondMedian { get; set; }

    public long HighestHitsTakenBlockedPerSecondRunID { get; set; }

    public double HighestActionsPerMinute { get; set; }

    public double ActionsPerMinuteAverage { get; set; }

    public double ActionsPerMinuteMedian { get; set; }

    public long HighestActionsPerMinuteRunID { get; set; }

    public long TotalHitsCount { get; set; }

    public long TotalHitsTakenBlocked { get; set; }

    public long TotalActions { get; set; }

    public double HealthAverage { get; set; }

    public double HealthMedian { get; set; }

    public double HealthMode { get; set; }

    public double StaminaAverage { get; set; }

    public double StaminaMedian { get; set; }

    public double StaminaMode { get; set; }
}
