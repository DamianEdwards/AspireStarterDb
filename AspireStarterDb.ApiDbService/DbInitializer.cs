using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace AspireStarterDb.ApiDbService;

internal class DbInitializer
{
    private DbInitializer() { }

    public const string ActivitySourceName = nameof(DbInitializer);
}

internal class DbInitializer<TDbContext>(IServiceProvider serviceProvider) : BackgroundService
    where TDbContext : DbContext
{
    private static readonly ActivitySource _activitySource = new(DbInitializer.ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity($"Initializing database for {typeof(TDbContext).Name}", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

            // Update to the latest migration. This will create the database if it doesn't exist.
            // For more control over the migration process, consider using the MigrateAsync overload that accepts a specific migration name.
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }
    }
}
