// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.Converter;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// Actual info in regards to the bingo points would be in Statistics tab (sandbox).
/// </summary>
public class BingoMonsterInfoToToolTipConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BingoMonster monster)
        {
            if (monster.IsUnlimited)
            {
                return "Any UL non-custom quest";
            }
            else
            {
                if (monster.QuestIDs == null)
                {
                    return string.Empty;
                }

                var mapper = EZlion.Mapper.Quest.IDName;
                var mappedStrings = monster.QuestIDs.Select(i => mapper.ContainsKey(i) ? mapper[i] : i.ToString());
                return string.Join(" | ", mappedStrings);
            }
        }

        return string.Empty;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
