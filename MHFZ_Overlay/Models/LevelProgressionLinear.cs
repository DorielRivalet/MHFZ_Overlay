// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

/// <summary>
/// The rate of change is constant. It forms a straight line on a graph.
/// </summary>
public sealed class LevelProgressionLinear
{
    /// <summary>
    /// The initial value of the first level.
    /// </summary>
    public decimal InitialValue { get; set; }

    /// <summary>
    /// The linear increase of the values. This factor determines how much the value increases with each level. For example, if ValueIncreasePerLevel is 10, then the value will increase by 10 with each level.
    /// </summary>
    public decimal ValueIncreasePerLevel { get; set; }

    /// <summary>
    /// Calculate the value based on the level.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public decimal CalculateLinearValueForLevel(int level)
    {
        return InitialValue + (ValueIncreasePerLevel * (level - 1));
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
            cumulativeValue += CalculateLinearValueForLevel(level);
        }
        return cumulativeValue;
    }
}
