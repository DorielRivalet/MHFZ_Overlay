// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Globalization;
using System.Windows.Data;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;

public class AchievementRankToBrushConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AchievementRank rank)
        {
            return rank switch
            {
                AchievementRank.Bronze => CatppuccinMochaColors.NameHex["Maroon"],
                AchievementRank.Silver => CatppuccinMochaColors.NameHex["Lavender"],
                AchievementRank.Gold => CatppuccinMochaColors.NameHex["Yellow"],
                AchievementRank.Platinum => CatppuccinMochaColors.NameHex["Teal"],
                AchievementRank.None => CatppuccinMochaColors.NameHex["Base"],
                _ => CatppuccinMochaColors.NameHex["Base"],
            };
        }

        return CatppuccinMochaColors.NameHex["Base"];
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
