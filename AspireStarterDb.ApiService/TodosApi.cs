using Microsoft.EntityFrameworkCore;
using AspireStarterDb.ApiDbModel;

namespace Microsoft.Extensions.Hosting;

public static class TodosApi
{
    public static IEndpointRouteBuilder MapTodosApi(this IEndpointRouteBuilder app)
    {
        var todos = app.MapGroup("/todos");

        todos.MapGet("/", (TodosDbContext db) => db.Todos);

        todos.MapGet("/{id:int}", (int id, TodosDbContext db) => db.Todos.FirstOrDefaultAsync(t => t.Id == id));

        return app;
    }
}
