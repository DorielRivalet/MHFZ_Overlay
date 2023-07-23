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
    /// The amount of bronze achievements required in order to unlock the challenge
    /// </summary>
    public int AchievementsBronzeRequired { get; set; }

    /// <summary>
    /// The amount of silver achievements required in order to unlock the challenge
    /// </summary>
    public int AchievementsSilverRequired { get; set; }

    /// <summary>
    /// The amount of gold achievements required in order to unlock the challenge
    /// </summary>
    public int AchievementsGoldRequired { get; set; }

    /// <summary>
    /// The amount of platinum achievements required in order to unlock the challenge
    /// </summary>
    public int AchievementsPlatinumRequired { get; set; }

    /// <summary>
    /// The achievement ID required (e.g. to unlock zenith gauntlet, you should beat gasura solo first). It should be a valid ID.
    /// </summary>
    public int AchievementIDRequired { get; set; }
}
