using Discord;
using MHFZ_Overlay.UI.Class;
using System;

namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class QuestCompendium
    {
        public int MostCompletedQuestRuns { get; set; }
        public int MostCompletedQuestRunsAttempted { get; set; }
        public int MostCompletedQuestRunsQuestID { get; set; }
        public int MostAttemptedQuestRuns { get; set; }
        public int MostAttemptedQuestRunsCompleted { get; set; }
        public int MostAttemptedQuestRunsQuestID { get; set; }
        public int TotalQuestsCompleted { get; set; }
        public int TotalQuestsAttempted { get; set; }
        public double QuestCompletionTimeElapsedAverage { get; set; }
        public double QuestCompletionTimeElapsedMedian { get; set; }
        public int TotalTimeElapsedQuests { get; set; }
        public double TotalCartsInQuestsAverage { get; set; }
        public double TotalCartsInQuestsMedian { get; set; }
        public int MostCompletedQuestWithCarts { get; set; }
        public int MostCompletedQuestWithCartsQuestID { get; set; }
        public int TotalCartsInQuest { get; set; }
        public double TotalCartsInQuestAverage { get; set; }
        public double TotalCartsInQuestMedian { get; set; }
        public double QuestPartySizeAverage { get; set; }
        public double QuestPartySizeMedian { get; set; }
        public double QuestPartySizeMode { get; set; }
        public double PercentOfSoloQuests { get; set; }
        public int PercentOfGuildFood { get; set; }
        public int PercentOfDivaSkill { get; set; }
        public int PercentOfSkillFruit { get; set; }
        public int MostCommonDivaSkill { get; set; }
        public int MostCommonGuildFood { get; set; }
    }
}
