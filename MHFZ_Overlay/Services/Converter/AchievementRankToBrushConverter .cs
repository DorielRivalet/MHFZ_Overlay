// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHFZ_Overlay.Models.Structures;
using System.Windows.Data;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;

public class AchievementRankToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AchievementRank rank)
        {
            switch (rank)
            {
                case AchievementRank.Bronze:
                    return CatppuccinMochaColors.NameHex["Maroon"];
                case AchievementRank.Silver:
                    return CatppuccinMochaColors.NameHex["Lavender"];
                case AchievementRank.Gold:
                    return CatppuccinMochaColors.NameHex["Yellow"];
                case AchievementRank.Platinum:
                    return CatppuccinMochaColors.NameHex["Teal"];
                default:
                    return CatppuccinMochaColors.NameHex["Base"];
            }
        }
        return CatppuccinMochaColors.NameHex["Base"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
