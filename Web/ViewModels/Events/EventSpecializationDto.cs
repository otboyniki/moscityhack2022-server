using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class EventSpecializationDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Requirements { get; set; }
    public string Description { get; set; } = null!;

    public bool IsOnline { get; set; }
    public IntRange? Ages { get; set; }

    public int MinVolunteersNumber { get; set; }
    public int MaxVolunteersNumber { get; set; }
    public bool IsRegisteredVolunteersNeeded { get; set; }

    public bool? IsParticipant { get; set; }
    public int TotalParticipants { get; set; }
    public int ConfirmedParticipants { get; set; }
    public int ReservedParticipants { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    [NotMapped, JsonIgnore]
    public static Expression<Func<EventSpecialization, EventSpecializationDto>> Projection =>
        ConditionalProjection(null);

    [NotMapped, JsonIgnore]
    public static Func<Guid?, Expression<Func<EventSpecialization, EventSpecializationDto>>>
        ConditionalProjection => userId => s => new EventSpecializationDto
        {
            Id = s.Id,

            Title = s.Title,
            Requirements = s.Requirements,
            Description = s.Description,

            IsOnline = s.IsOnline,
            Ages = s.Ages,

            MinVolunteersNumber = s.MinVolunteersNumber,
            MaxVolunteersNumber = s.MaxVolunteersNumber,
            IsRegisteredVolunteersNeeded = s.IsRegisteredVolunteersNeeded,

            IsParticipant = userId.HasValue
                ? s.Participants
                   .AsQueryable()
                   .Any(x => x.VolunteerId == userId.Value)
                : null,
            TotalParticipants = s.Participants.Count,
            ConfirmedParticipants = s.Participants
                                     .Count(x => x.IsConfirmed),
            ReservedParticipants = s.Participants
                                    .Count(x => !x.IsMember),

            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
}