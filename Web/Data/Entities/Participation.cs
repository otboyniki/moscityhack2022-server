using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

/// <summary>
///     Участие в мероприятии
/// </summary>
[Table("Participants")]
public class Participation : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;

    public Guid VolunteerId { get; set; }
    public User Volunteer { get; set; } = null!;

    /// <summary>
    ///     Подтверждение волонтёром
    /// </summary>
    public bool Confirmed { get; set; }

    /// <remarks>
    ///     Если false, значит чел в резерве
    /// </remarks>
    public bool IsMember { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}