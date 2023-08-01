// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Globalization;
using System.Windows.Data;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;

public class AchievementToColorConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "#000000";
        }

        var achievement = (Achievement)value;

        if (achievement.CompletionDate == DateTime.UnixEpoch && achievement.IsSecret)
        {
            return CatppuccinMochaColors.NameHex["Text"];
        }
        else
        {
            // insert your logic here to return the color based on the rank of the achievement
            // for example, return new SolidColorBrush(Colors.Red);
            return achievement.Rank switch
            {
                AchievementRank.Bronze => CatppuccinMochaColors.NameHex["Maroon"],
                AchievementRank.Silver => CatppuccinMochaColors.NameHex["Lavender"],
                AchievementRank.Gold => CatppuccinMochaColors.NameHex["Yellow"],
                AchievementRank.Platinum => CatppuccinMochaColors.NameHex["Teal"],
                AchievementRank.None => throw new NotImplementedException(),
                _ => CatppuccinMochaColors.NameHex["Base"],
            };
        }
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
