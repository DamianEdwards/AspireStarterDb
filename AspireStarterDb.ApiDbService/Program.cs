using AspireStarterDb.ApiDbModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire integrations.
builder.AddServiceDefaults();
builder.AddTodosDbContext("todosdb");

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapPost("/reset-db", async (TodosDbContext db) =>
    {
        // Delete and recreate the database. This is useful for development scenarios to reset the database to its initial state.
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
    });
}

app.MapDefaultEndpoints();

app.Run();
