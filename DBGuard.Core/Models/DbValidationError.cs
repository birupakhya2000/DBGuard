using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Models
{
    public class DbValidationError
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string ErrorType { get; set; }

        public string Message { get; set; }

        public int? AllowedLength { get; set; }

        public int? ActualLength { get; set; }
        public string Suggestion { get; set; }
    }
}
