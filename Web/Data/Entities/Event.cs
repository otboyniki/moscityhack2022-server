using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

/// <summary>
///     Мероприятие
/// </summary>
[Table("Events")]
public class Event : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public Guid? PreviewId { get; set; }
    public File Preview { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Terms { get; set; } = null!;

    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    public ICollection<Address> Locations { get; set; } = new List<Address>();

    public DateTimeRange? Recruitment { get; set; }
    public DateTimeRange Meeting { get; set; } = null!;
    public string? MeetingNote { get; set; }

    public virtual ICollection<EventSpecialization> Specializations { get; set; } = new List<EventSpecialization>();
    public virtual ICollection<EventReview> EventReviews { get; set; } = new List<EventReview>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}