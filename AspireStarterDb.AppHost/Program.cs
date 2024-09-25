var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisCommander();

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var todosDb = postgres.AddDatabase("todosdb");

var apiService = builder.AddProject<Projects.AspireStarterDb_ApiService>("apiservice")
    .WithReference(todosDb);

builder.AddProject<Projects.AspireStarterDb_ApiDbService>("apidbservice")
    .WithReference(todosDb);

builder.AddProject<Projects.AspireStarterDb_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
