// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

/// <summary>
/// Affected by player stats.
/// </summary>
public sealed class MonsterLog
{
    public MonsterLog(int id, string name, string image, int hunted, bool islarge = false)
    {
        this.ID = id;
        this.Name = name;
        this.MonsterImage = image;
        this.Hunted = hunted;
        this.IsLarge = islarge;
    }

    public string Name { get; set; }

    public int ID { get; set; }

    public int Hunted { get; set; }

    public bool IsLarge { get; set; }

    public string MonsterImage { get; set; }
}
