// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Windows.Data;

namespace MHFZ_Overlay.UI.Class.Converter
{
    /// <summary>
    /// Create a Value Converter to disable the Up & Down Arrow buttons of the scrollbar
    /// when the Thumb reaches the minimum & maximum position on the scroll track.
    /// </summary>

    public class ScrollLimitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double && values[1] is double)
            {
                return (double)values[0] == (double)values[1];
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
