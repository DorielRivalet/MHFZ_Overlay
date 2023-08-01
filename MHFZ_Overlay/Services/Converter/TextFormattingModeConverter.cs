// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

public sealed class TextFormattingModeConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string textFormattingModeString)
        {
            if (Enum.TryParse(textFormattingModeString, out TextFormattingMode textFormattingMode))
            {
                return textFormattingMode;
            }
        }

        return TextFormattingMode.Ideal;
    }

    /// <inheritdoc/>
    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TextFormattingMode textFormattingMode)
        {
            return textFormattingMode.ToString();
        }

        return null;
    }
}
