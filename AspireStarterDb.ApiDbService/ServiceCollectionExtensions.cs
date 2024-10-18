using Microsoft.EntityFrameworkCore;
using AspireStarterDb.ApiDbModel;
using AspireStarterDb.ApiDbService;

namespace Microsoft.Extensions.Hosting;

internal static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddTodosDbContext(this IHostApplicationBuilder builder, string connectionName)
    {
        builder.Services.AddDbContextPool<TodosDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString(connectionName);

            // Use this app's assembly for migrations, rather than the assembly containing the DbContext.
            //options.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(typeof(Program).Assembly.GetName()?.Name));
            options.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(typeof(Program).Assembly.GetName()?.Name));

            // Add sample seed data. Learn more about seeding data at https://learn.microsoft.com/ef/core/modeling/data-seeding
            options.UseAsyncSeeding(TodosDbContext.SeedAsync);
        });

        //builder.EnrichNpgsqlDbContext<TodosDbContext>();
        builder.EnrichSqlServerDbContext<TodosDbContext>();

        builder.AddDbInitializer<TodosDbContext>();

        return builder;
    }

    private static IHostApplicationBuilder AddDbInitializer<TDbContext>(this IHostApplicationBuilder builder)
        where TDbContext : DbContext
    {
        builder.Services.AddSingleton<DbInitializer<TDbContext>>();

        builder.Services.AddHostedService(sp => sp.GetRequiredService<DbInitializer<TDbContext>>());

        builder.Services.AddHealthChecks()
            .AddCheck<DbInitializerHealthCheck<TDbContext>>($"{typeof(TDbContext).Name} initialization");

        return builder;
    }
}
