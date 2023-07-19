// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;
using System.Globalization;
using System.Windows.Data;
using System;

public class CompletionDateToOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime completionDate && completionDate == DateTime.UnixEpoch)
        {
            return 0.25;
        }

        return 1.0; // Default opacity
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
