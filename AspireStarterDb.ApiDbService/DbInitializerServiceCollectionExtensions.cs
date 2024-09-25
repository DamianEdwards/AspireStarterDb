using Microsoft.EntityFrameworkCore;

namespace AspireStarterDb.ApiDbService;

internal static class DbInitializerServiceCollectionExtensions
{
    public static IServiceCollection AddDbInitializer<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddSingleton<DbInitializer<TDbContext>>();
        services.AddHostedService(sp => sp.GetRequiredService<DbInitializer<TDbContext>>());
        services.AddHealthChecks()
            .AddCheck<DbInitializerHealthCheck<TDbContext>>($"{typeof(TDbContext).Name} initialization");
        return services;
    }
}
