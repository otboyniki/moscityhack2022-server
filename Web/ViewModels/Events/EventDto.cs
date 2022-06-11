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

    public string CompanyName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Terms { get; set; } = null!;
    public string Kind { get; set; } = null!;

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
    public static Expression<Func<Event, EventDto>> Projection => evt => new EventDto
    {
        Id = evt.Id,
        CompanyId = evt.CompanyId,
        PreviewId = evt.PreviewId,

        CompanyName = evt.Company.Title,
        Title = evt.Title,
        Description = evt.Description,
        Kind = evt.Kind,
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
                                   .Count(x => x.Confirmed),
        ReservedParticipants = evt.Specializations
                                  .SelectMany(x => x.Participants)
                                  .Count(x => !x.IsMember),

        Specializations = evt.Specializations
                             .AsQueryable()
                             .Select(EventSpecializationDto.Projection)
                             .ToList(),

        CreatedAt = evt.CreatedAt,
        UpdatedAt = evt.UpdatedAt
    };
}