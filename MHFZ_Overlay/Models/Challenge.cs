// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MHFZ_Overlay.Models.Structures;

public sealed class Challenge
{
    /// <summary>
    /// The name of the challenge (Bingo, Gacha, Gauntlets)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Summary of the challenge
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The challenge data template to load when selecting the challenge to start.
    /// </summary>
    public DataTemplateKey? ChallengeDataTemplateKey { get; set; }

    /// <summary>
    /// The amount of achievements required in order to unlock the challenge
    /// </summary>
    public int AchievementsRequired { get; set; }

    /// <summary>
    /// The rank of the achievements required in order to unlock the challenge.
    /// If set to none, it does not require any particular achievement type and any achievement counts for the requirement.
    /// </summary>
    public AchievementRank AchievementRankRequired { get; set; }

    /// <summary>
    /// The achievement ID required (e.g. to unlock zenith gauntlet, you should beat gasura solo first). It should be a valid ID.
    /// </summary>
    public int AchievementIDRequired { get; set; }
}
