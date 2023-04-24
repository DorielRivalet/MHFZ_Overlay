namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class PerformanceCompendium
    {
        public int HighestTrueRaw { get; set; }
        public double TrueRawAverage { get; set; }
        public double TrueRawMedian { get; set; }
        public int HighestTrueRawRunID { get; set; }
        public int HighestSingleHitDamage { get; set; }
        public double SingleHitDamageAverage { get; set; }
        public double SingleHitDamageMedian { get; set; }
        public int HighestSingleHitDamageRunID { get; set; }
        public int HighestHitCount { get; set; }
        public double HitCountAverage { get; set; }
        public double HitCountMedian { get; set; }
        public int HighestHitCountRunID { get; set; }
        public int HighestHitsTakenBlocked { get; set; }
        public double HitsTakenBlockedAverage { get; set; }
        public double HitsTakenBlockedMedian { get; set; }
        public int HighestHitsTakenBlockedRunID { get; set; }
        public double HighestDPS { get; set; }
        public double DPSAverage { get; set; }
        public double DPSMedian { get; set; }
        public int HighestDPSRunID { get; set; }
        public double HighestHitsPerSecond { get; set; }
        public double HitsPerSecondAverage { get; set; }
        public double HitsPerSecondMedian { get; set; }
        public int HighestHitsPerSecondRunID { get; set; }
        public double HighestHitsTakenBlockedPerSecond { get; set; }
        public double HitsTakenBlockedPerSecondAverage { get; set; }
        public double HitsTakenBlockedPerSecondMedian { get; set; }
        public int HighestHitsTakenBlockedPerSecondRunID { get; set; }
        public double HighestActionsPerMinute { get; set; }
        public double ActionsPerMinuteAverage { get; set; }
        public double ActionsPerMinuteMedian { get; set; }
        public int HighestActionsPerMinuteRunID { get; set; }
        public int TotalHitsCount { get; set; }
        public int TotalHitsTakenBlocked { get; set; }
        public int TotalActions { get; set; }
        public double HealthAverage { get; set; }
        public double HealthMedian { get; set; }
        public double HealthMode { get; set; }
        public double StaminaAverage { get; set; }
        public double StaminaMedian { get; set; }
        public double StaminaMode { get; set; }
        public int TotalLargeMonstersHunted { get; set; }
        public int TotalSmallMonstersHunted { get; set; }
    }
}
