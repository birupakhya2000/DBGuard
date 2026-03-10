using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Exceptions
{
    public class DbGuardValidationException : Exception
    {
        public List<DbValidationError> Errors { get; }

        public DbGuardValidationException(List<DbValidationError> errors)
            : base(BuildMessage(errors))
        {
            Errors = errors;
        }

        private static string BuildMessage(List<DbValidationError> errors)
        {
            var message = "DBGuard Validation Failed\n\n";

            foreach (var error in errors)
            {
                message += $"Table: {error.TableName}\n";
                message += $"Column: {error.ColumnName}\n";
                message += $"Error: {error.Message}\n";

                if (error.AllowedLength.HasValue)
                    message += $"Allowed Length: {error.AllowedLength}\n";

                if (error.ActualLength.HasValue)
                    message += $"Actual Length: {error.ActualLength}\n";

                message += "\n";
            }

            return message;
        }
    }
}
