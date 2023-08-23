// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Globalization;
using System.Windows.Data;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;

public class GauntletBoostToImageConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is GauntletBoost boost)
        {
            if (boost.HasFlag(GauntletBoost.Zenith) && boost.HasFlag(GauntletBoost.Solstice) && boost.HasFlag(GauntletBoost.Musou))
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_max.png";
            }
            else if (boost.HasFlag(GauntletBoost.Zenith) && boost.HasFlag(GauntletBoost.Solstice))
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_orange.png";
            }
            else if (boost.HasFlag(GauntletBoost.Zenith) && boost.HasFlag(GauntletBoost.Musou))
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_purple.png";
            }
            else if (boost.HasFlag(GauntletBoost.Solstice) && boost.HasFlag(GauntletBoost.Musou))
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_cyan.png";
            }
            else if (boost.HasFlag(GauntletBoost.Zenith))
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_red.png";
            }
            else if (boost.HasFlag(GauntletBoost.Solstice))
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_green.png";
            }
            else if (boost.HasFlag(GauntletBoost.Musou))
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_blue.png";
            }
            else
            {
                return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_white.png";
            }
        }

        return @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/gauntlet_white.png";
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
