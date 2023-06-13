// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;

namespace MHFZ_Overlay.UI.Class;

//TODO: ORM
// get the graphs from here
public class Quest
{
    public string QuestHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public long RunID { get; set; }
    public long QuestID { get; set; }
    public long TimeLeft { get; set; }
    public long FinalTimeValue { get; set; }
    public string FinalTimeDisplay { get; set; }
    public string ObjectiveImage { get; set; }

    public long ObjectiveTypeID { get; set; }
    public long ObjectiveQuantity { get; set; }
    public long StarGrade { get; set; }

    public string RankName { get; set; }
    public string ObjectiveName { get; set; }
    public DateTime Date { get; set; }

    public string YouTubeID { get; set; }
    public string AttackBuffDictionary { get; set; }
    public string HitCountDictionary { get; set; }
    public string HitsPerSecondDictionary { get; set; }

    public string DamageDealtDictionary { get; set; }
    public string DamagePerSecondDictionary { get; set; }
    public string AreaChangesDictionary { get; set; }
    public string CartsDictionary { get; set; }

    public string Monster1HPDictionary { get; set; }
    public string Monster2HPDictionary { get; set; }
    public string Monster3HPDictionary { get; set; }
    public string Monster4HPDictionary { get; set; }

    public string HitsTakenBlockedDictionary { get; set; }
    public string HitsTakenBlockedPerSecondDictionary { get; set; }
    public string PlayerHPDictionary { get; set; }
    public string PlayerStaminaDictionary { get; set; }
    public string KeyStrokesDictionary { get; set; }
    public string MouseInputDictionary { get; set; }
    public string GamepadInputDictionary { get; set; }

    public string ActionsPerMinuteDictionary { get; set; }
    public string OverlayModeDictionary { get; set; }
    public string ActualOverlayMode { get; set; }
    public long PartySize { get; set; }
}
