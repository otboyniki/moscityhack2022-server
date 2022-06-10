using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

[Table("CommunicationVerifications")]
public class Verification : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public Guid CommunicationId { get; set; }
    public Communication Communication { get; set; } = null!;

    public string Code { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}