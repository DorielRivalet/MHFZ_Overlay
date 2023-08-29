// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHFZ_Overlay.Models.Structures;

public sealed class BingoMonster
{
    /// <summary>
    /// The name of the monster
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The type of the monster. Used to determine the scraps type to give.
    /// </summary>
    public FrontierMonsterType Type { get; set; }

    /// <summary>
    /// The image of the monster
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The list of quest IDs of the monster
    /// </summary>
    public List<int>? QuestIDs { get; set; }

    /// <summary>
    /// Whether the monster to hunt should be in Unlimited mode. If so, the QuestIDs are ignored if any and any non-custom quest counts as a completion.
    /// </summary>
    public bool IsUnlimited { get; set; }

    /// <summary>
    /// The monster ID of the unlimited monster.
    /// </summary>
    public int UnlimitedMonsterID { get; set; }

    /// <summary>
    /// The base bingo score obtained if defeated the monster.
    /// </summary>
    public int BaseScore { get; set; }
}
