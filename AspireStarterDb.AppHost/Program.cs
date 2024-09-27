using AspireStarterDb.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisInsight();

//var dbServer = builder.AddPostgres("postgres")
//    .WithPgAdmin();

var dbServer = builder.AddSqlServer("sqlserver");

var todosDb = dbServer.AddDatabase("todosdb");

// The ApiDbService project is responsible for managing the database schema and seeding data.
var apiDbService = builder.AddProject<Projects.AspireStarterDb_ApiDbService>("apidbservice")
    .WithReference(todosDb)
    .WaitFor(todosDb)
    .WithHttpsHealthCheck("/health");

// The ApiService project provides backend HTTP APIs for the web frontend.
var apiService = builder.AddProject<Projects.AspireStarterDb_ApiService>("apiservice")
    .WithReference(todosDb)
    .WaitFor(apiDbService);

// The Web project is a Blazor web frontend.
builder.AddProject<Projects.AspireStarterDb_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WaitFor(cache);

builder.Build().Run();
