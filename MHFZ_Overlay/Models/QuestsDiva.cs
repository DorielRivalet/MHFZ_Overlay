// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

// TODO: ORM
public sealed class QuestsDiva
{
    public long? QuestsDivaID { get; set; }
    public long? DivaSongBuffOn { get; set; }
    public long? DivaPrayerGemRedSkill { get; set; }
    public long? DivaPrayerGemRedLevel { get; set; }
    public long? DivaPrayerGemYellowSkill { get; set; }
    public long? DivaPrayerGemYellowLevel { get; set; }
    public long? DivaPrayerGemGreenSkill { get; set; }
    public long? DivaPrayerGemGreenLevel { get; set; }
    public long? DivaPrayerGemBlueSkill { get; set; }
    public long? DivaPrayerGemBlueLevel { get; set; }
    public long? RunID { get; set; }
}
