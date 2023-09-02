// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

public class QuestIDsToNamesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is List<int> intList)
        {
            var mapper = EZlion.Mapper.Quest.IDName;
            var mappedStrings = intList.Select(i => mapper.ContainsKey(i) ? mapper[i] : i.ToString());
            return string.Join(" | ", mappedStrings);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

