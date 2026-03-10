using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Models
{
    public class DatabaseSchema
    {
        public Dictionary<string, TableSchema> Tables { get; set; }
            = new(StringComparer.OrdinalIgnoreCase);
    }
}
