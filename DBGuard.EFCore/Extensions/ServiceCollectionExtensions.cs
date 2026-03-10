using DBGuard.Core.Diagnostics;
using DBGuard.Core.Interfaces;
using DBGuard.EFCore.Interceptors;
using DBGuard.Schema.Cache;
using DBGuard.SqlServer.Providers;
using DBGuard.Validation.Engine;
using DBGuard.Validation.Validators;
using Microsoft.Extensions.DependencyInjection;


namespace DBGuard.EFCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbGuard(
            this IServiceCollection services,
            string connectionString)
        {
            var provider = new SqlServerSchemaProvider();

            var schema = provider.LoadSchemaAsync(connectionString)
                                 .GetAwaiter()
                                 .GetResult();

            SchemaCache.Set(connectionString, schema);

            services.AddSingleton(schema);

            services.AddSingleton<IColumnValidator, LengthValidator>();
            services.AddSingleton<IColumnValidator, NullValidator>();
            services.AddSingleton<IColumnValidator, DataTypeValidator>();

            services.AddSingleton<IValidationEngine, ValidationEngine>();

            services.AddSingleton<DbGuardInterceptor>();
            services.AddSingleton<ValidationMetrics>();

            services.AddSingleton<IColumnValidator, DecimalPrecisionValidator>();
            services.AddSingleton<IColumnValidator, NumericOverflowValidator>();
            services.AddSingleton<IColumnValidator, DateValidator>();

            return services;
        }
    }
}
