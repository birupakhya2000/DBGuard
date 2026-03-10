using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Models
{
    public class ColumnSchema
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public int? MaxLength { get; set; }

        public bool IsNullable { get; set; }

        public int? Precision { get; set; }

        public int? Scale { get; set; }
    }
}
