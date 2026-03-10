using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Interfaces
{
    public interface IValidationEngine
    {
        ValidationResult Validate(string tableName, Dictionary<string, object> values);
    }
}
