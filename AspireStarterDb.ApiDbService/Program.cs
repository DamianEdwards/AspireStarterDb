using Microsoft.EntityFrameworkCore;
using AspireStarterDb.ApiDbModel;
using AspireStarterDb.ApiDbService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContextPool<TodosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("todosdb"), sqlOptions =>
    {
        // Configure the DbContext to use this app's assembly for migrations, rather than
        // the assembly containing the DbContext.
        sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
    });

    // Add sample seed data. Learn more about seeding data at https://learn.microsoft.com/ef/core/modeling/data-seeding
    TodosDbContext.ConfigureSeeding(options);
});
builder.EnrichNpgsqlDbContext<TodosDbContext>();

// Enable tracing for the database initializer.
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DbInitializer.ActivitySourceName));

// Register the database initializer and health check.
builder.Services.AddDbInitializer<TodosDbContext>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
