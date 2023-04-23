using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System;

namespace MHFZ_Overlay {
    public class TextFormattingModeConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TextFormattingMode textFormattingMode)
            {
                return textFormattingMode.ToString();
            }

            return null;
        }
    }

}
