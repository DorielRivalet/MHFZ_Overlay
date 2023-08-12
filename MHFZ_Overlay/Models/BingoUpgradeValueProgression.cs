// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

public sealed class BingoUpgradeValueProgression
{
    /// <summary>
    /// The initial value of the first upgrade.
    /// </summary>
    public double InitialValue { get; set; }

    /// <summary>
    /// The linear increase of the upgrades values. This factor determines how much the value increases with each level. For example, if ValueIncreasePerLevel is 10, then the value will increase by 10 with each level.
    /// </summary>
    public double ValueIncreasePerLevel { get; set; }

    /// <summary>
    /// Calculate the value based on the level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public double CalculateValue(int level)
    {
        return InitialValue + (ValueIncreasePerLevel * (level - 1));
    }
}
