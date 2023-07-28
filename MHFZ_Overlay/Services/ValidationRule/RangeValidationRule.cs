// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Services.ValidationRule;

using System.Globalization;
using System.Windows.Controls;

public sealed class RangeValidationRule : ValidationRule
{
    public int Minimum { get; set; }

    public int Maximum { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (int.TryParse((string)value, out var inputValue))
        {
            if (inputValue < this.Minimum || inputValue > this.Maximum)
            {
                return new ValidationResult(false, $"Value must be between {this.Minimum} and {this.Maximum}");
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
