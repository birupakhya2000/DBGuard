using DBGuard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.Core.Interfaces
{
    public interface ISchemaProvider
    {
        Task<DatabaseSchema> LoadSchemaAsync(string connectionString);
    }
}
