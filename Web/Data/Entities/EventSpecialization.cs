using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

[Table("EventSpecializations")]
public class EventSpecialization : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Requirements { get; set; }
    public string Description { get; set; } = null!;

    public bool IsOnline { get; set; }
    public IntRange? Ages { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int MinVolunteersNumber { get; set; }
    public int MaxVolunteersNumber { get; set; }
    public bool IsRegisteredVolunteersNeeded { get; set; }

    public virtual ICollection<Participation> Participants { get; set; } = new List<Participation>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}