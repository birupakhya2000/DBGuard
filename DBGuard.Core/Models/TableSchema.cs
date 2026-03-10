using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Models
{
    public class TableSchema
    {
        public string TableName { get; set; }

        public Dictionary<string, ColumnSchema> Columns { get; set; }
            = new(StringComparer.OrdinalIgnoreCase);
    }
}
