using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AtlasPadder
{
    public class TileSizeValidator : ValidationRule
    {
        public override ValidationResult Validate
      (object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Tile size cannot be empty.");
            else
            {
                int tileSize;
                if(!int.TryParse(value.ToString(), out tileSize))
                {
                    return new ValidationResult
                    (false, "Must be an integer greater than 0");
                }
                else if(tileSize < 0)
                {
                    return new ValidationResult
                    (false, "Must be an integer greater than 0");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
