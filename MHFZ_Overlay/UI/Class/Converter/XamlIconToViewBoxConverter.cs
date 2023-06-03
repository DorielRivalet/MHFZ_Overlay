// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace MHFZ_Overlay.UI.Class.Converter;

class XamlIconToViewBoxConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            // code to execute when the application is in design mode
            return null;
        }

        var stream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/" + (string)parameter)).Stream;
        var viewBox = XamlReader.Load(stream) as Viewbox;

        // Optional:
        // we set Height and Width to "Auto" to let an icon scale, because in the <icon>.xaml file its size is explicitly specified as 16x16
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        viewBox.Height = double.NaN;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        viewBox.Width = double.NaN;

        return viewBox;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
