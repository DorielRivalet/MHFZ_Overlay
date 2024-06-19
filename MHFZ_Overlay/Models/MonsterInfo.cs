// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System.Collections.Generic;

/// <summary>
/// Unaffected by player stats.
/// </summary>
public sealed class MonsterInfo
{
    public MonsterInfo(string name, string feriaslink, Dictionary<string, string> matchups, string wikilink, string wycademyLink)
    {
        this.Name = name;
        this.FeriasLink = feriaslink;
        this.WeaponMatchups = matchups;
        this.WikiLink = wikilink;
        this.WycademyLink = wycademyLink;
    }

    public string Name { get; set; }

    public Dictionary<string, string> WeaponMatchups { get; set; }

    public string WikiLink { get; set; }

    public string FeriasLink { get; set; }

    public string WycademyLink { get; set; }

}
