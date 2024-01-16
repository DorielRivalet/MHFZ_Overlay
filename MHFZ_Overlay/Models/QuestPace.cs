// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System.Collections.Generic;

/// <summary>
/// The pace of the quest.
/// </summary>
public sealed class QuestPace
{
    public long RunID { get; set; }

    public Dictionary<int, Dictionary<int, int>>? MonsterHPField { get; set; }

    public Dictionary<int, int>? MonsterHPFieldFlattened { get; set; }

    public QuestSplit? Splits { get; set; }
   
    public QuestObjectiveSplit? ObjectiveSplits { get; set; }

}
