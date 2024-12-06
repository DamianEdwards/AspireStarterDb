using Microsoft.EntityFrameworkCore;

namespace AspireStarterDb.ApiDbModel;

public class TodosDbContext(DbContextOptions<TodosDbContext> options) : DbContext(options)
{
    public const string TodosUniqueIndex = $"IX_{nameof(Todos)}_Unique";

    public required DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var todoBuilder = modelBuilder.Entity<Todo>()
            .ToTable(b => b.HasCheckConstraint("CK_CompletedOn",
                // SQL Server
                $"[{nameof(Todo.CompletedOn)}] = NULL OR [{nameof(Todo.CompletedOn)}] > [{nameof(Todo.CreatedOn)}]"));
                // PostgreSQL
                //$"\"{nameof(Todo.CompletedOn)}\" IS NULL OR \"{nameof(Todo.CompletedOn)}\" > \"{nameof(Todo.CreatedOn)}\""));
        todoBuilder.HasIndex(e => new { e.Title, e.CompletedOn }, TodosUniqueIndex)
            .IsUnique().HasFilter(null);
        todoBuilder.Property(t => t.CreatedOn)
            // SQL Server
            .HasDefaultValueSql("GETUTCDATE()");
            // PostgreSQL
            //.HasDefaultValueSql("CURRENT_TIMESTAMP");
    }

    public static async Task SeedAsync(DbContext dbContext, bool storeManagementPerformed, CancellationToken cancellationToken)
    {
        var todosDbContext = (TodosDbContext)dbContext;

        if (!await todosDbContext.Todos.AnyAsync(cancellationToken))
        {
            todosDbContext.Todos.AddRange(
                new Todo { Title = "Mow the lawn" },
                new Todo { Title = "Take out the trash" },
                new Todo { Title = "Vacuum the house", CompletedOn = DateTime.UtcNow.AddDays(-4) },
                new Todo { Title = "Wash the car" },
                new Todo { Title = "Clean the gutters", CompletedOn = DateTime.UtcNow.AddDays(-7) }
            );

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
