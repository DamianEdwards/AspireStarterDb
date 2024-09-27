using AspireStarterDb.ApiDbModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire integrations.
builder.AddServiceDefaults();

builder.AddSqlServerDbContext<TodosDbContext>("todosdb");
//builder.AddNpgsqlDbContext<TodosDbContext>("todosdb");

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapWeatherApi();

app.MapTodosApi();

app.MapDefaultEndpoints();

app.Run();
