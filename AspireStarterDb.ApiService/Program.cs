using AspireStarterDb.ApiDbModel;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire integrations.
builder.AddServiceDefaults();
//builder.AddNpgsqlDbContext<TodosDbContext>("todosdb");
builder.AddSqlServerDbContext<TodosDbContext>("todosdb");

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapWeatherApi();

app.MapTodosApi();

app.MapDefaultEndpoints();

app.Run();
