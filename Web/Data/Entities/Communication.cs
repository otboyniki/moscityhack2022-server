using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Enums;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

[Table("Communications")]
public class Communication : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public CommunicationType Type { get; set; }
    public string Value { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}