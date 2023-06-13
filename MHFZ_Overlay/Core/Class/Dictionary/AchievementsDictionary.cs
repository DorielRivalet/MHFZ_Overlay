// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MHFZ_Overlay.UI.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFZ_Overlay.Core.Class.Dictionary;

/// <summary>
/// Achievements dictionary. TODO
/// </summary>
public static class AchievementsDictionary
{
    /// <summary>
    /// Gets the achievement list.
    /// </summary>
    /// <value>
    /// The achievement list.
    /// </value>
    public static IReadOnlyDictionary<int, Achievement> IDAchievement { get; } = new Dictionary<int, Achievement>
    {
        {0, new Achievement(){
            CompletionDate = DateTime.MinValue,
            Description = string.Empty,
            Hint = string.Empty,
            Rank = Enum.AchievementRank.Bronze,
            }
        },
    };
}
