using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MHFZ_Overlay.UI.Class
{
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
}
