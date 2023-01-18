using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace MHFZ_Overlay
{
    class XamlIconToViewBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stream = System.Windows.Application.GetResourceStream(new Uri((string)parameter, UriKind.Relative)).Stream;
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
}
