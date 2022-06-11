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

    public int MinVolunteersNumber { get; set; }
    public int MaxVolunteersNumber { get; set; }
    public bool IsRegisteredVolunteersNeeded { get; set; }

    public int TotalParticipants { get; set; }
    public int ConfirmedParticipants { get; set; }
    public int ReservedParticipants { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    [NotMapped, JsonIgnore]
    public static Expression<Func<EventSpecialization, EventSpecializationDto>> Projection => s => new EventSpecializationDto
    {
        Id = s.Id,

        Title = s.Title,
        Requirements = s.Requirements,
        Description = s.Description,

        MinVolunteersNumber = s.MinVolunteersNumber,
        MaxVolunteersNumber = s.MaxVolunteersNumber,
        IsRegisteredVolunteersNeeded = s.IsRegisteredVolunteersNeeded,

        TotalParticipants = s.Participants.Count,
        ConfirmedParticipants = s.Participants
                                 .Count(x => x.IsConfirmed),
        ReservedParticipants = s.Participants
                                .Count(x => !x.IsMember),

        CreatedAt = s.CreatedAt,
        UpdatedAt = s.UpdatedAt
    };
}