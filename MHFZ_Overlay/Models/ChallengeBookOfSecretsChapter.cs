// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Memory;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

/// <summary>
/// The challenge book of secrets upgrade.
/// </summary>
public sealed class ChallengeBookOfSecretsChapter
{
    /// <summary>
    /// The name of the chapter.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the chapter.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The text shown after obtaining the chapter.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// The amount of pages required for the chapter.
    /// </summary>
    public int PagesRequired { get; set; }

    /// <summary>
    /// Whether the chapter is unlocked.
    /// </summary>
    public bool IsUnlocked { get; set; }

    /// <summary>
    /// The list of chapters this chapter unlocks.
    /// </summary>
    public List<ChallengeBookOfSecretsChapter> UnlockedChapters { get; set; } = new ();

    /// <summary>
    /// The list of upgrades this chapter unlocks.
    /// </summary>
    public List<object> UnlockedUpgrades { get; set; } = new();

    /// <summary>
    /// The required chapters to unlock this chapter.
    /// </summary>
    public List<ChallengeBookOfSecretsChapter> ChaptersRequired { get; set; } = new();

    /// <summary>
    /// The parts required for the chapter.
    /// </summary>
    public List<ChallengeAncientDragonPart> ChallengeAncientDragonPartsRequired { get; set; } = new();
}
