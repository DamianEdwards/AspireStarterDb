using Microsoft.EntityFrameworkCore;

namespace AspireStarterDb.ApiDbModel;

public class TodosDbContext(DbContextOptions<TodosDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add sample seed data.
        // Learn more about seeding data at https://learn.microsoft.com/ef/core/modeling/data-seeding
        modelBuilder.Entity<Todo>().HasData(
            new Todo { Id = 1, Title = "Walk the dog", IsComplete = false },
            new Todo { Id = 2, Title = "Do the dishes", IsComplete = false },
            new Todo { Id = 3, Title = "Do the laundry", IsComplete = true },
            new Todo { Id = 4, Title = "Clean the bathroom", IsComplete = false },
            new Todo { Id = 5, Title = "Clean the car", IsComplete = true }
        );
    }
}
