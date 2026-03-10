using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Validation.Validators
{
    public class NullValidator : IColumnValidator
    {
        public ValidationResult Validate(ColumnSchema column, object value)
        {
            if (!column.IsNullable && value == null)
            {
                return ValidationResult.Failure(new DbValidationError
                {
                    TableName = column.TableName,
                    ColumnName = column.ColumnName,
                    ErrorType = "NullViolation",
                    Message = $"{column.ColumnName} cannot be null"
                });
            }

            return ValidationResult.Success();
        }
    }
}
