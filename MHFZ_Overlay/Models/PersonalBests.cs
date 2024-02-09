// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;

public sealed class PersonalBests
{
    public PersonalBests(string weaponType, long attempts, DateTime createdAt, string actualOverlayMode, long runID, long timeLeft, long runBuffs)
    {
        this.WeaponType = weaponType;
        this.Attempts = attempts;
        this.CreatedAt = createdAt;
        this.ActualOverlayMode = actualOverlayMode;
        this.RunID = runID;
        this.TimeLeft = timeLeft;
        this.RunBuffs = runBuffs;
    }

    public string WeaponType { get; set; }

    public long Attempts { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ActualOverlayMode { get; set; }

    public long RunID { get; set; }

    public long TimeLeft { get; set; }

    public long RunBuffs { get; set; }

}
