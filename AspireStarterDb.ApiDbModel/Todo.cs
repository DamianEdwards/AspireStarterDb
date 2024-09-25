using System.ComponentModel.DataAnnotations;

namespace AspireStarterDb.ApiDbModel;

public class Todo
{
    public int Id { get; set; }

    [Required]
    [StringLength(1000)]
    public required string Title { get; set; }

    public bool IsComplete { get; set; }
}
