using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class EventDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? PreviewId { get; set; }

    public Guid ActivityId { get; set; }
    public string ActivityName { get; set; } = null!;

    public string CompanyName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Terms { get; set; } = null!;

    public ICollection<AddressDto> Locations { get; set; } = new List<AddressDto>();

    public DateTimeRange? Recruitment { get; set; }
    public DateTimeRange Meeting { get; set; } = null!;
    public string? MeetingNote { get; set; }

    public int MinVolunteersNumber { get; set; }
    public int MaxVolunteersNumber { get; set; }
    public bool IsRegisteredVolunteersNeeded { get; set; }

    public int TotalParticipants { get; set; }
    public int ConfirmedParticipants { get; set; }
    public int ReservedParticipants { get; set; }

    public ICollection<EventSpecializationDto> Specializations { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    [NotMapped, JsonIgnore]
    public static Expression<Func<Event, EventDto>> Projection => ConditionalProjection(null, _ => true);

    [NotMapped, JsonIgnore]
    public static Func<Guid?, Expression<Func<EventSpecialization, bool>>, Expression<Func<Event, EventDto>>>
        ConditionalProjection => (userId, condition) => evt => new EventDto
    {
        Id = evt.Id,
        CompanyId = evt.CompanyId,
        PreviewId = evt.PreviewId,
        ActivityId = evt.ActivityId,
        ActivityName = evt.Activity.Title,

        CompanyName = evt.Company.Title,
        Title = evt.Title,
        Description = evt.Description,
        Terms = evt.Terms,

        Locations = evt.Locations
                       .AsQueryable()
                       .Select(AddressDto.Projection)
                       .ToList(),
        Recruitment = evt.Recruitment,
        Meeting = evt.Meeting,
        MeetingNote = evt.MeetingNote,

        MinVolunteersNumber = evt.Specializations
                                 .Sum(x => x.MinVolunteersNumber),
        MaxVolunteersNumber = evt.Specializations
                                 .Sum(x => x.MaxVolunteersNumber),
        IsRegisteredVolunteersNeeded = evt.Specializations
                                          .Any(x => x.IsRegisteredVolunteersNeeded),

        TotalParticipants = evt.Specializations
                               .SelectMany(x => x.Participants)
                               .Count(),
        ConfirmedParticipants = evt.Specializations
                                   .SelectMany(x => x.Participants)
                                   .Count(x => x.IsConfirmed),
        ReservedParticipants = evt.Specializations
                                  .SelectMany(x => x.Participants)
                                  .Count(x => !x.IsMember),

        Specializations = evt.Specializations
                             .AsQueryable()
                             .Where(condition)
                             .Select(EventSpecializationDto.ConditionalProjection(userId))
                             .ToList(),

        CreatedAt = evt.CreatedAt,
        UpdatedAt = evt.UpdatedAt
    };
}