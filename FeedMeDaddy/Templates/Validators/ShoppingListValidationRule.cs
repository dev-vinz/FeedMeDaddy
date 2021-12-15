using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FeedMeDaddy.Templates.Validators
{
	public class ShoppingListValidationRule : ValidationRule
	{
		public Type ValidationType { get; set; }

		public ShoppingListValidationRule()
		{
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string strValue = Convert.ToString(value);

			if (string.IsNullOrEmpty(strValue))
				return new ValidationResult(false, $"Value cannot be converted to string.");

			bool canConvert = false;

			switch (ValidationType.Name)
			{
				case "Boolean":
					canConvert = bool.TryParse(strValue, out bool boolVal);
					return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Boolean");
				case "Int32":
				case "Double":
				case "Int64":
					canConvert = long.TryParse(strValue, out long longVal);
					return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
				default:
					throw new InvalidCastException($"{ValidationType.Name} is not supported");
			}
		}
	}
}
