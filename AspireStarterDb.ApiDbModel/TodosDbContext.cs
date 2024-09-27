using Microsoft.EntityFrameworkCore;

namespace AspireStarterDb.ApiDbModel;

public class TodosDbContext(DbContextOptions<TodosDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }

    public static async Task SeedAsync(DbContext dbContext, bool storeManagementPerformed, CancellationToken cancellationToken)
    {
        var todosDbContext = (TodosDbContext)dbContext;

        if (!await todosDbContext.Todos.AnyAsync(cancellationToken))
        {
            todosDbContext.Todos.AddRange(
                new Todo { Title = "Mow the lawn", IsComplete = false },
                new Todo { Title = "Take out the trash", IsComplete = false },
                new Todo { Title = "Vacuum the house", IsComplete = true },
                new Todo { Title = "Wash the car", IsComplete = false },
                new Todo { Title = "Clean the gutters", IsComplete = true }
            );

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
