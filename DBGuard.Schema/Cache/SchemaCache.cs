using DBGuard.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Schema.Cache
{
    public class SchemaCache
    {
        private static readonly ConcurrentDictionary<string, DatabaseSchema> _cache = new();

        public static void Set(string connectionString, DatabaseSchema schema)
        {
            _cache[connectionString] = schema;
        }

        public static DatabaseSchema Get(string connectionString)
        {
            _cache.TryGetValue(connectionString, out var schema);
            return schema;
        }
    }
}
