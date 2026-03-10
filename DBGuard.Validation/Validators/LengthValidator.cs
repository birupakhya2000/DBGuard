using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Validation.Validators
{
    public class LengthValidator : IColumnValidator
    {
        public ValidationResult Validate(ColumnSchema column, object value)
        {
            if (value == null)
                return ValidationResult.Success();

            var str = value.ToString();

            if (column.MaxLength.HasValue && str.Length > column.MaxLength.Value)
            {
                return ValidationResult.Failure(new DbValidationError
                {
                    TableName = column.TableName,
                    ColumnName = column.ColumnName,
                    ErrorType = "LengthExceeded",
                    AllowedLength = column.MaxLength,
                    ActualLength = str.Length,
                    Message = $"{column.ColumnName} exceeds max length {column.MaxLength}. " + "Suggestion: Trim the input or increase the column size."
                });
            }

            return ValidationResult.Success();
        }
    }
}
