using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Validation.Validators
{
    public class DateValidator : IColumnValidator
    {
        public ValidationResult Validate(ColumnSchema column, object value)
        {
            if (value == null)
                return ValidationResult.Success();

            if (!column.DataType.Contains("date"))
                return ValidationResult.Success();

            try
            {
                Convert.ToDateTime(value);
            }
            catch
            {
                return ValidationResult.Failure(new DbValidationError
                {
                    TableName = column.TableName,
                    ColumnName = column.ColumnName,
                    ErrorType = "InvalidDate",
                    Message = $"{column.ColumnName} expects a valid date"
                });
            }

            return ValidationResult.Success();
        }
    }
}
