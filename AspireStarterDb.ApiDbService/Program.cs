using AspireStarterDb.ApiDbService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddTodosDbContext("todosdb");

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
