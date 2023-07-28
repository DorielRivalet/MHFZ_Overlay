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
using MHFZ_Overlay.Models;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;

public class AchievementToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "#000000";
        }

        Achievement achievement = (Achievement)value;

        if (achievement.CompletionDate == DateTime.UnixEpoch && achievement.IsSecret)
        {
            return CatppuccinMochaColors.NameHex["Text"];
        }
        else
        {
            // insert your logic here to return the color based on the rank of the achievement
            // for example, return new SolidColorBrush(Colors.Red);
            switch (achievement.Rank)
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
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
