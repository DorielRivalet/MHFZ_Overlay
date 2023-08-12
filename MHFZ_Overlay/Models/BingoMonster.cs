// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class BingoMonster
{
    /// <summary>
    /// The name of the monster
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The image of the monster
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The list of quest IDs of the monster
    /// </summary>
    public List<int>? QuestIDs { get; set; }

    /// <summary>
    /// The base bingo score obtained if defeated the monster.
    /// </summary>
    public int BaseScore { get; set; }
}
