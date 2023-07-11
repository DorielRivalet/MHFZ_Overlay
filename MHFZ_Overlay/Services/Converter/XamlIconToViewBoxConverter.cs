// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

sealed class XamlIconToViewBoxConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            // code to execute when the application is in design mode
            return null;
        }

        var stream = Application.GetResourceStream(new Uri("pack://application:,,,/" + (string)parameter)).Stream;
        var viewBox = XamlReader.Load(stream) as Viewbox;

        // Optional:
        // we set Height and Width to "Auto" to let an icon scale, because in the <icon>.xaml file its size is explicitly specified as 16x16
        viewBox.Height = double.NaN;
        viewBox.Width = double.NaN;

        return viewBox;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
