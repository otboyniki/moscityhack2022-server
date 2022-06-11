using System.ComponentModel.DataAnnotations;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class File : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;

    [MaxLength(256)]
    public string ContentType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}