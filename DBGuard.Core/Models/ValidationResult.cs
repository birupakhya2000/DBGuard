using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public List<DbValidationError> Errors { get; set; } = new();

        public static ValidationResult Success()
        {
            return new ValidationResult { IsValid = true };
        }

        public static ValidationResult Failure(DbValidationError error)
        {
            return new ValidationResult
            {
                IsValid = false,
                Errors = new List<DbValidationError> { error }
            };
        }
    }
}
