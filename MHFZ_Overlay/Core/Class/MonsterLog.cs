// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Core.Class;

/// <summary>
/// Affected by player stats
/// </summary>
public class MonsterLog
{

    public string Name { get; set; }
    public int ID { get; set; }
    public int Hunted { get; set; }

    public bool IsLarge { get; set; }
    public string MonsterImage { get; set; }

    public MonsterLog(int id, string name, string image, int hunted, bool islarge = false)
    {
        ID = id;
        Name = name;
        MonsterImage = image;
        Hunted = hunted;
        IsLarge = islarge;
    }
}
