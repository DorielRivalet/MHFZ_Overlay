// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

// TODO: ORM
public sealed class MezFesCompendium
{
    public long MinigamesPlayed { get; set; }

    public long UrukiPachinkoTimesPlayed { get; set; }

    public long UrukiPachinkoHighscore { get; set; }

    public double UrukiPachinkoAverageScore { get; set; }

    public double UrukiPachinkoMedianScore { get; set; }

    public long GuukuScoopTimesPlayed { get; set; }

    public long GuukuScoopHighscore { get; set; }

    public double GuukuScoopAverageScore { get; set; }

    public double GuukuScoopMedianScore { get; set; }

    public long NyanrendoTimesPlayed { get; set; }

    public long NyanrendoHighscore { get; set; }

    public double NyanrendoAverageScore { get; set; }

    public double NyanrendoMedianScore { get; set; }

    public long PanicHoneyTimesPlayed { get; set; }

    public long PanicHoneyHighscore { get; set; }

    public double PanicHoneyAverageScore { get; set; }

    public double PanicHoneyMedianScore { get; set; }

    public long DokkanBattleCatsTimesPlayed { get; set; }

    public long DokkanBattleCatsHighscore { get; set; }

    public double DokkanBattleCatsAverageScore { get; set; }

    public double DokkanBattleCatsMedianScore { get; set; }
}
