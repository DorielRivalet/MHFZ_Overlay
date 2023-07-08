// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

using System.Globalization;
using System.Windows.Controls;

public class RangeValidationRule : ValidationRule
{
    public int Minimum { get; set; }

    public int Maximum { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var inputValue = 0;

        if (int.TryParse((string)value, out inputValue))
        {
            if (inputValue < Minimum || inputValue > Maximum)
            {
                return new ValidationResult(false, $"Value must be between {Minimum} and {Maximum}");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
        else
        {
            return new ValidationResult(false, "Value must be a valid integer");
        }
    }
}
