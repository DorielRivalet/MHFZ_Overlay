// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

// TODO: ORM
public sealed class MonsterCompendium
{
    public double HighestMonsterAttackMultiplier { get; set; }

    public double HighestMonsterAttackMultiplierRunID { get; set; }

    public double LowestMonsterAttackMultiplier { get; set; }

    public double LowestMonsterAttackMultiplierRunID { get; set; }

    public double HighestMonsterDefenseRate { get; set; }

    public double HighestMonsterDefenseRateRunID { get; set; }

    public double LowestMonsterDefenseRate { get; set; }

    public double LowestMonsterDefenseRateRunID { get; set; }

    public double HighestMonsterSizeMultiplier { get; set; }

    public double HighestMonsterSizeMultiplierRunID { get; set; }

    public double LowestMonsterSizeMultiplier { get; set; }

    public double LowestMonsterSizeMultiplierRunID { get; set; }
}
