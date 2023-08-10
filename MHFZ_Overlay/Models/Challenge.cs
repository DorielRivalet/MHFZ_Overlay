// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Windows;
using System.Windows.Input;

public sealed class Challenge
{
    public ICommand? StartChallengeCommand { get; set; }

    /// <summary>
    /// Gets or sets the link to the banner image.
    /// </summary>
    public string BannerImageLink { get; set; } = @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/achievement/award_gold.png";

    /// <summary>
    /// Gets or sets the name of the challenge (Bingo, Gacha, Gauntlets).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets summary of the challenge.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the challenge data template to load when selecting the challenge to start.
    /// </summary>
    public DataTemplateKey? ChallengeDataTemplateKey { get; set; }

    /// <summary>
    /// Gets or sets the amount of bronze achievements required in order to unlock the challenge.
    /// </summary>
    public int AchievementsBronzeRequired { get; set; }

    /// <summary>
    /// Gets or sets the amount of silver achievements required in order to unlock the challenge.
    /// </summary>
    public int AchievementsSilverRequired { get; set; }

    /// <summary>
    /// Gets or sets the amount of gold achievements required in order to unlock the challenge.
    /// </summary>
    public int AchievementsGoldRequired { get; set; }

    /// <summary>
    /// Gets or sets the amount of platinum achievements required in order to unlock the challenge.
    /// </summary>
    public int AchievementsPlatinumRequired { get; set; }

    /// <summary>
    /// Gets or sets the achievement ID required (e.g. to unlock zenith gauntlet, you should beat gasura solo first). It should be a valid ID.
    /// </summary>
    public int AchievementIDRequired { get; set; }

    /// <summary>
    /// Gets or sets the name of the achievement id required.
    /// </summary>
    public string AchievementNameRequired { get; set; } = string.Empty;

    /// <summary>
    /// The date when the challenge was unlocked
    /// </summary>
    public DateTime UnlockDate { get; set; } = DateTime.UnixEpoch;
}
