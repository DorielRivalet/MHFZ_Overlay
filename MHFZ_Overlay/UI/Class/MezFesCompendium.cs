using Discord;
using MHFZ_Overlay.UI.Class;
using System;

namespace MHFZ_Overlay.UI.Class
{
    //TODO: ORM
    public class MezFesCompendium
    {
        public int MinigamesPlayed { get; set; }
        public int UrukiPachinkoTimesPlayed { get; set; }
        public int UrukiPachinkoHighscore { get; set; }
        public double UrukiPachinkoAverageScore { get; set; }
        public double UrukiPachinkoMedianScore { get; set; }
        public int GuukuScoopTimesPlayed { get; set; }
        public int GuukuScoopHighscore { get; set; }
        public double GuukuScoopAverageScore { get; set; }
        public double GuukuScoopMedianScore { get; set; }
        public int NyanrendoTimesPlayed { get; set; }
        public int NyanrendoHighscore { get; set; }
        public double NyanrendoAverageScore { get; set; }
        public double NyanrendoMedianScore { get; set; }
        public int PanicHoneyTimesPlayed { get; set; }
        public int PanicHoneyHighscore { get; set; }
        public double PanicHoneyAverageScore { get; set; }
        public double PanicHoneyMedianScore { get; set; }
        public int DokkanBattleCatsTimesPlayed { get; set; }
        public int DokkanBattleCatsHighscore { get; set; }
        public double DokkanBattleCatsAverageScore { get; set; }
        public double DokkanBattleCatsMedianScore { get; set; }
    }
}
