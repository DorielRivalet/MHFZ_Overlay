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
/// The rate of change follows a power function, where it's raised to a constant exponent.
/// </summary>
public sealed class LevelProgressionPower
{
    /// <summary>
    /// The initial value of the first level.
    /// </summary>
    public int InitialValue { get; set; }

    /// <summary>
    /// The exponent that determines the rate of change. This factor determines how the value changes with each level. For example, if Exponent is 2, then the value will increase squared with each level.
    /// </summary>
    public double Exponent { get; set; }

    /// <summary>
    /// Calculate the value based on the level.
    /// </summary>
    /// <param name="level"></param>
    /// <returns>The calculated value.</returns>
    public int CalculatePowerValueForLevel(int level)
    {
        return (int)Math.Ceiling(InitialValue * Math.Pow(level, Exponent));
    }

    /// <summary>
    /// Assumes the initial level starts at 1.
    /// </summary>
    /// <param name="maxLevel"></param>
    /// <returns>The cumulative value at max level.</returns>
    public int CalculateCumulativeValueForMaxLevel(int maxLevel)
    {
        var cumulativeValue = 0;
        for (int level = 1; level <= maxLevel; level++)
        {
            cumulativeValue += CalculatePowerValueForLevel(level);
        }
        return cumulativeValue;
    }
}

// POWER LEVELS
