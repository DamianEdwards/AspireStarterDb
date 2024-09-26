using Microsoft.EntityFrameworkCore;

namespace AspireStarterDb.ApiDbModel;

public class TodosDbContext(DbContextOptions<TodosDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }

    public static void ConfigureSeeding(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseAsyncSeeding(SeedAsync);
        optionsBuilder.UseSeeding(Seed);
    }

    private static async Task SeedAsync(DbContext dbContext, bool storeManagementPerformed, CancellationToken cancellationToken)
    {
        var todosDbContext = (TodosDbContext)dbContext;
        if (!await todosDbContext.Todos.AnyAsync(cancellationToken))
        {
            AddSeedTodos(todosDbContext);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static void Seed(DbContext dbContext, bool storeManagementPerformed)
    {
        var todosDbContext = (TodosDbContext)dbContext;
        if (!todosDbContext.Todos.Any())
        {
            AddSeedTodos(todosDbContext);
            dbContext.SaveChanges();
        }
    }

    private static void AddSeedTodos(TodosDbContext dbContext)
    {
        dbContext.Todos.AddRange(
            new Todo { Id = 1, Title = "Mow the lawn", IsComplete = false },
            new Todo { Id = 2, Title = "Take out the trash", IsComplete = false },
            new Todo { Id = 3, Title = "Vacuum the house", IsComplete = true },
            new Todo { Id = 4, Title = "Wash the car", IsComplete = false },
            new Todo { Id = 5, Title = "Clean the gutters", IsComplete = true }
        );
    }
}
