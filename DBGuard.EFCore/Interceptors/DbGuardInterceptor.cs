using System.Threading;
using DBGuard.Core.Diagnostics;
using DBGuard.Core.Exceptions;
using DBGuard.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGuard.EFCore.Interceptors
{
    public class DbGuardInterceptor : SaveChangesInterceptor
    {
        private readonly IValidationEngine _validationEngine;
        private readonly ILogger<DbGuardInterceptor> _logger;
        private readonly ValidationMetrics _metrics;

        public DbGuardInterceptor(
            IValidationEngine validationEngine,
            ILogger<DbGuardInterceptor> logger,
            ValidationMetrics metrics)
        {
            _validationEngine = validationEngine;
            _logger = logger;
            _metrics = metrics;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context == null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            var entries = context.ChangeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                ValidateEntity(entry);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ValidateEntity(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            var tableName = entry.Metadata.GetTableName();

            var values = new Dictionary<string, object>();

            foreach (var prop in entry.Properties)
            {
                if (prop.IsModified || entry.State == EntityState.Added)
                {
                    values[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            var result = _validationEngine.Validate(tableName, values);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _metrics.RecordFailure(error.TableName, error.ColumnName);
                    _logger.LogWarning(
                        "DBGuard validation failed. Table: {Table}, Column: {Column}, Error: {Error}",
                        error.TableName,
                        error.ColumnName,
                        error.Message);
                }

                throw new DbGuardValidationException(result.Errors);
            }
        }
    }
}
