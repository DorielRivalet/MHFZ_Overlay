// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// TODO
/// </summary>
public class BingoPlayer
{
    /// <summary>
    /// The amount of bingo points currently stored.
    /// </summary>
    public int BingoPoints { get; set; }

    /// <summary>
    /// The amount of book of secrets pages currently stored.
    /// </summary>
    public int BookOfSecretsPages { get; set; }

    /// <summary>
    /// The amount of unlocked book of secrets chapters.
    /// </summary>
    public List<ChallengeBookOfSecretsChapter>? UnlockedChapters { get; set; }

    /// <summary>
    /// The amount of bingo shop upgrades unlocked.
    /// </summary>
    public List<BingoUpgrade>? UnlockedUpgrades { get; set; }
}
