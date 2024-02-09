// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

// TODO: ORM
public sealed class QuestCompendium
{
    public long MostCompletedQuestRuns { get; set; }

    public long MostCompletedQuestRunsAttempted { get; set; }

    public long MostCompletedQuestRunsQuestID { get; set; }

    public long MostAttemptedQuestRuns { get; set; }

    public long MostAttemptedQuestRunsCompleted { get; set; }

    public long MostAttemptedQuestRunsQuestID { get; set; }

    public long TotalQuestsCompleted { get; set; }

    public long TotalQuestsAttempted { get; set; }

    public double QuestCompletionTimeElapsedAverage { get; set; }

    public double QuestCompletionTimeElapsedMedian { get; set; }

    public long TotalTimeElapsedQuests { get; set; }

    public double TotalCartsInQuestsAverage { get; set; }

    public double TotalCartsInQuestsMedian { get; set; }

    public long MostCompletedQuestWithCarts { get; set; }

    public long MostCompletedQuestWithCartsQuestID { get; set; }

    public long TotalCartsInQuest { get; set; }

    public double TotalCartsInQuestAverage { get; set; }

    public double TotalCartsInQuestMedian { get; set; }

    public double QuestPartySizeAverage { get; set; }

    public double QuestPartySizeMedian { get; set; }

    public long QuestPartySizeMode { get; set; }

    public double PercentOfSoloQuests { get; set; }

    public double PercentOfGuildFood { get; set; }

    public double PercentOfDivaSkill { get; set; }

    public double PercentOfSkillFruit { get; set; }

    public double PercentOfActiveFeature { get; set; }

    public double PercentOfDivaSong { get; set; }

    public double PercentOfDivaPrayerGem { get; set; }

    public double PercentOfHalkOn { get; set; }

    public double PercentOfHalkPotEffectOn { get; set; }

    public double PercentOfCourseAttackBoost { get; set; }

    public double PercentOfGuildPoogie { get; set; }

    public long MostCommonGuildPoogie { get; set; }

    public long MostCommonDivaSkill { get; set; }

    public long MostCommonGuildFood { get; set; }
}
