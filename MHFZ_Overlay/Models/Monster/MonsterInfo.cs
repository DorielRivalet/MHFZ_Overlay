// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Monster;

using System.Collections.Generic;


/// <summary>
/// Unaffected by player stats
/// </summary>
public class MonsterInfo
{
    public MonsterInfo(string name, string feriaslink, Dictionary<string, string> matchups, string wikilink)
    {
        this.Name = name;
        this.FeriasLink = feriaslink;
        this.WeaponMatchups = matchups;
        this.WikiLink = wikilink;
    }

    public string Name { get; set; }

    public Dictionary<string, string> WeaponMatchups { get; set; }

    public string WikiLink { get; set; }

    public string FeriasLink { get; set; }
}
