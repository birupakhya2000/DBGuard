using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Validation.Validators
{
    public class DecimalPrecisionValidator : IColumnValidator
    {
        public ValidationResult Validate(ColumnSchema column, object value)
        {
            if (value == null)
                return ValidationResult.Success();

            if (column.DataType != "decimal" && column.DataType != "numeric")
                return ValidationResult.Success();

            if (column.Precision == null || column.Scale == null)
                return ValidationResult.Success();

            try
            {
                var decimalValue = Convert.ToDecimal(value);

                var parts = decimalValue.ToString().Split('.');

                var integerDigits = parts[0].Replace("-", "").Length;
                var decimalDigits = parts.Length > 1 ? parts[1].Length : 0;

                if (integerDigits + decimalDigits > column.Precision ||
                    decimalDigits > column.Scale)
                {
                    return ValidationResult.Failure(new DbValidationError
                    {
                        TableName = column.TableName,
                        ColumnName = column.ColumnName,
                        ErrorType = "DecimalPrecisionExceeded",
                        Message =
                            $"{column.ColumnName} exceeds decimal precision ({column.Precision},{column.Scale})"
                    });
                }
            }
            catch
            {
                return ValidationResult.Success();
            }

            return ValidationResult.Success();
        }
    }
}
