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

    public Guid EventSpecializationId { get; set; }
    public EventSpecialization EventSpecialization { get; set; } = null!;

    public Guid VolunteerId { get; set; }
    public VolunteerUser Volunteer { get; set; } = null!;

    /// <summary>
    ///     Подтверждение волонтёром
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    ///     Организатор отметил что волонтёр явился на мероприятие
    /// </summary>
    public bool IsAttended { get; set; }

    /// <remarks>
    ///     Если false, значит чел в резерве
    /// </remarks>
    public bool IsMember { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}