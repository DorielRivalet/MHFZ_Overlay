// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;

// TODO: ORM
// get the graphs from here
public sealed class Quest
{
    public string? QuestHash { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; } = string.Empty;

    public long? RunID { get; set; }

    public long QuestID { get; set; }

    public long? TimeLeft { get; set; }

    public long? FinalTimeValue { get; set; }

    public string? FinalTimeDisplay { get; set; } = string.Empty;

    public string? ObjectiveImage { get; set; } = string.Empty;

    public long ObjectiveTypeID { get; set; }

    public long? ObjectiveQuantity { get; set; }

    public long? StarGrade { get; set; }

    public string? RankName { get; set; } = string.Empty;

    public string? ObjectiveName { get; set; } = string.Empty;

    public DateTime? Date { get; set; }

    public string? YouTubeID { get; set; } = string.Empty;

    public string? AttackBuffDictionary { get; set; } = string.Empty;

    public string? HitCountDictionary { get; set; } = string.Empty;

    public string? HitsPerSecondDictionary { get; set; } = string.Empty;

    public string? DamageDealtDictionary { get; set; } = string.Empty;

    public string? DamagePerSecondDictionary { get; set; } = string.Empty;

    public string? AreaChangesDictionary { get; set; } = string.Empty;

    public string? CartsDictionary { get; set; } = string.Empty;

    public string? Monster1HPDictionary { get; set; } = string.Empty;

    public string? Monster2HPDictionary { get; set; } = string.Empty;

    public string? Monster3HPDictionary { get; set; } = string.Empty;

    public string? Monster4HPDictionary { get; set; } = string.Empty;

    public string? HitsTakenBlockedDictionary { get; set; } = string.Empty;

    public string? HitsTakenBlockedPerSecondDictionary { get; set; } = string.Empty;

    public string? PlayerHPDictionary { get; set; } = string.Empty;

    public string? PlayerStaminaDictionary { get; set; } = string.Empty;

    public string? KeyStrokesDictionary { get; set; } = string.Empty;

    public string? MouseInputDictionary { get; set; } = string.Empty;

    public string? GamepadInputDictionary { get; set; } = string.Empty;

    public string? ActionsPerMinuteDictionary { get; set; } = string.Empty;

    public string? OverlayModeDictionary { get; set; } = string.Empty;

    public string? ActualOverlayMode { get; set; } = string.Empty;

    public long? PartySize { get; set; } = 0;

    public string? PartySizeDictionary {  get; set; } = string.Empty;

}
