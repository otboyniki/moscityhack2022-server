using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class File : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    [Required]
    public string Path { get; set; }

    [MaxLength(256)]
    public string ContentType { get; set; }

    [NotMapped]
    public byte[] Content { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}