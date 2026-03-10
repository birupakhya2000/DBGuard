using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Diagnostics
{
    public class ValidationMetrics
    {
        private readonly ConcurrentDictionary<string, int> _columnFailures = new();

        public void RecordFailure(string table, string column)
        {
            var key = $"{table}.{column}";

            _columnFailures.AddOrUpdate(key, 1, (_, count) => count + 1);
        }

        public Dictionary<string, int> GetTopFailures()
        {
            return _columnFailures
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
