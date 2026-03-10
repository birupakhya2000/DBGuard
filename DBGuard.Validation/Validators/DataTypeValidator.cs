using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Validation.Validators
{
    public class DataTypeValidator : IColumnValidator
    {
        public ValidationResult Validate(ColumnSchema column, object value)
        {
            if (value == null)
                return ValidationResult.Success();

            var type = column.DataType.ToLower();

            try
            {
                switch (type)
                {
                    case "int":
                        Convert.ToInt32(value);
                        break;

                    case "bigint":
                        Convert.ToInt64(value);
                        break;

                    case "decimal":
                    case "numeric":
                        Convert.ToDecimal(value);
                        break;

                    case "datetime":
                        Convert.ToDateTime(value);
                        break;
                }
            }
            catch
            {
                return ValidationResult.Failure(new DbValidationError
                {
                    TableName = column.TableName,
                    ColumnName = column.ColumnName,
                    ErrorType = "InvalidType",
                    Message = $"{column.ColumnName} expects {column.DataType}"
                });
            }

            return ValidationResult.Success();
        }
    }
}
