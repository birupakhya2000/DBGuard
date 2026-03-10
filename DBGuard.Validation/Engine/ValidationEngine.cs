using DBGuard.Core.Interfaces;
using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Validation.Engine
{
    public class ValidationEngine : IValidationEngine
    {
        private readonly DatabaseSchema _schema;
        private readonly IEnumerable<IColumnValidator> _validators;

        public ValidationEngine(DatabaseSchema schema,
            IEnumerable<IColumnValidator> validators)
        {
            _schema = schema;
            _validators = validators;
        }

        public ValidationResult Validate(string tableName,
            Dictionary<string, object> values)
        {
            if (!_schema.Tables.TryGetValue(tableName, out var table))
                throw new Exception($"Table {tableName} not found in schema");

            var errors = new List<DbValidationError>();

            foreach (var item in values)
            {
                if (!table.Columns.TryGetValue(item.Key, out var column))
                    continue;

                foreach (var validator in _validators)
                {
                    var result = validator.Validate(column, item.Value);

                    if (!result.IsValid)
                    {
                        errors.AddRange(result.Errors);
                    }
                }
            }

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }
    }
}
