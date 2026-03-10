using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Validation.Validators
{
    public class NumericOverflowValidator : IColumnValidator
    {
        public ValidationResult Validate(ColumnSchema column, object value)
        {
            if (value == null)
                return ValidationResult.Success();

            try
            {
                switch (column.DataType)
                {
                    case "int":
                        Convert.ToInt32(value);
                        break;

                    case "bigint":
                        Convert.ToInt64(value);
                        break;

                    case "smallint":
                        Convert.ToInt16(value);
                        break;
                }
            }
            catch
            {
                return ValidationResult.Failure(new DbValidationError
                {
                    TableName = column.TableName,
                    ColumnName = column.ColumnName,
                    ErrorType = "NumericOverflow",
                    Message = $"{column.ColumnName} value exceeds allowed numeric range"
                });
            }

            return ValidationResult.Success();
        }
    }
}
