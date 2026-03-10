using DBGuard.Core.Diagnostics;
using DBGuard.Demo;
using DBGuard.EFCore.Extensions;
using DBGuard.EFCore.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var connectionString = "server=192.168.1.17;user id=skeltadbusr;password=skelta@123;database=JournalItrack;TrustServerCertificate = True";

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(config =>
        {
            config.AddConsole();
        });

        services.AddDbGuard(connectionString);

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<DbGuardInterceptor>();

            options.UseSqlServer(connectionString)
                   .AddInterceptors(interceptor);
        });
    })
    .Build();

using var scope = host.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var user = new User
{
    FirstName = "THIS_IS_A_VERY_LONG_NAME_EXCEEDING_COLUMN_LIMIT_123456",
    Email = "test@test.com"
};

context.Users.Add(user);

await context.SaveChangesAsync();

var metrics = scope.ServiceProvider.GetRequiredService<ValidationMetrics>();

var failures = metrics.GetTopFailures();

foreach (var f in failures)
{
    Console.WriteLine($"{f.Key} -> {f.Value}");
}