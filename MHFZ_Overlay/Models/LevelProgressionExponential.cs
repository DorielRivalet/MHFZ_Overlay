// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The rate of change increases or decreases exponentially, often leading to rapid growth or decay.
/// </summary>
public sealed class LevelProgressionExponential
{
    /// <summary>
    /// The initial value of the first level.
    /// </summary>
    public decimal InitialValue { get; set; }

    /// <summary>
    /// The exponential increase of the value. This factor determines how much the value increases with each level. For example, if ValueIncreaseFactor is 1.5, then the value will increase by 50% with each level.
    /// </summary>
    public decimal ValueIncreaseFactor { get; set; }

    /// <summary>
    /// Calculates the value for that particular level.
    /// </summary>
    /// <param name="level"></param>
    /// <returns>The calculated value.</returns>
    public decimal CalculateExponentialValueForLevel(int level)
    {
        return (decimal)Math.Ceiling(InitialValue * (decimal)Math.Pow((double)ValueIncreaseFactor, level - 1));
    }

    /// <summary>
    /// Assumes the initial level starts at 1.
    /// </summary>
    /// <param name="maxLevel"></param>
    /// <returns>The cumulative value at max level.</returns>
    public decimal CalculateCumulativeValueForMaxLevel(int maxLevel)
    {
        var cumulativeValue = 0M;
        for (int level = 1; level <= maxLevel; level++)
        {
            cumulativeValue += CalculateExponentialValueForLevel(level);
        }
        return cumulativeValue;
    }
}
