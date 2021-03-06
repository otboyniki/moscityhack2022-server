using System.Linq.Expressions;
using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class ReviewDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;
    public Guid? AvatarId { get; set; }

    public EventSummaryDto EventSummary { get; set; } = null!;

    public string Text { get; set; } = null!;

    public int CompanyRate { get; set; }
    public int GoalComplianceRate { get; set; }

    public int PositiveScore { get; set; }
    public int NiggativeScore { get; set; }
    public bool? MyScore { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    public static Func<Guid, Expression<Func<EventReview, ReviewDto>>> Projection => userId =>
        r => new ReviewDto
        {
            Id = r.Id,

            Username = r.User.FirstName!,
            AvatarId = r.User.AvatarId,

            EventSummary = new EventSummaryDto
            {
                EventId = r.EventId,
                PreviewId = r.Event.PreviewId,
                Title = r.Event.Title,
                Meeting = r.Event.Meeting,
                Locations = r.Event.Locations
                             .Select(x => new AddressDto
                             {
                                 StringLocation = x.StringLocation,
                                 PointLocation = x.PointLocation != null
                                     ? new PointDto(x.PointLocation)
                                     : null
                             })
                             .ToList()
            },

            Text = r.Text,

            CompanyRate = r.CompanyRate,
            GoalComplianceRate = r.GoalComplianceRate,

            PositiveScore = r.ReviewScores.Count(x => x.Positive),
            NiggativeScore = r.ReviewScores.Count(x => !x.Positive),
            MyScore = r.ReviewScores
                       .FirstOrDefault(x => x.UserId == userId)!
                       .Positive,

            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt
        };
}