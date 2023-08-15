// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Globalization;
using System.Windows.Data;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;

public class FrontierWeaponTypeToIconsConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is FrontierWeaponType type)
        {
            return type switch
            {
                FrontierWeaponType.GreatSword => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_gs.png",
                FrontierWeaponType.LongSword => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_ls.png",
                FrontierWeaponType.DualSwords => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_ds.png",
                FrontierWeaponType.SwordAndShield => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_sns.png",
                FrontierWeaponType.Hammer => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_hammer.png",
                FrontierWeaponType.HuntingHorn => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_hh.png",
                FrontierWeaponType.Lance => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_lance.png",
                FrontierWeaponType.Gunlance => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_gl.png",
                FrontierWeaponType.LightBowgun => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_lbg.png",
                FrontierWeaponType.HeavyBowgun => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_hbg.png",
                FrontierWeaponType.Bow =>      @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_bow.png ",
                FrontierWeaponType.Tonfa => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_tonfa.png",
                FrontierWeaponType.SwitchAxeF => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_saf.png",
                FrontierWeaponType.MagnetSpike => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/small_ms.png",
                _ => @"pack://application:,,,/MHFZ_Overlay;component/Assets/Icons/png/unknown.png",
            };
        }

        return CatppuccinMochaColors.NameHex["Base"];
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
