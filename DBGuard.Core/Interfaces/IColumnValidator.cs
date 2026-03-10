using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Interfaces
{
    public interface IColumnValidator
    {
        ValidationResult Validate(ColumnSchema column, object value);
    }
}
