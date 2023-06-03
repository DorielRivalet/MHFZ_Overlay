// © 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Globalization;
using System.Windows.Controls;

namespace MHFZ_Overlay.UI.Class;

public class RangeValidationRule : ValidationRule
{
    public int Minimum { get; set; }
    public int Maximum { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        int inputValue = 0;

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
