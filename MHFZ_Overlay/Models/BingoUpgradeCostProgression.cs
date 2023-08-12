// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class BingoUpgradeCostProgression
{
    /// <summary>
    /// The initial cost of the first upgrade.
    /// </summary>
    public int InitialCost { get; set; }

    /// <summary>
    /// The exponential increase of the upgrades. This factor determines how much the cost increases with each level. For example, if CostIncreaseFactor is 1.5, then the cost will increase by 50% with each level.
    /// </summary>
    public double CostIncreaseFactor { get; set; }

    public int CalculateUpgradeCost(int level)
    {
        return (int)Math.Ceiling(InitialCost * Math.Pow(CostIncreaseFactor, level - 1));
    }
}
