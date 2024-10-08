using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AspireStarterDb.ApiDbModel;

[Index(nameof(CreatedOn))]
public class Todo
{
    public int Id { get; set; }

    [Required]
    [StringLength(1000)]
    public required string Title { get; set; }

    public DateTime CreatedOn { get; }

    public DateTime? CompletedOn { get; set; }

    public bool IsComplete => CompletedOn.HasValue;
}
