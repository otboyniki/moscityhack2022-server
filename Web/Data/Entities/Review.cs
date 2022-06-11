using Web.Data.Interfaces;

namespace Web.Data.Entities;

public abstract class Review : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<ReviewScore> ReviewScores { get; set; } = new List<ReviewScore>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Comment : Review
{
    public Guid StoryId { get; set; }
    public Story Story { get; set; } = null!;
}

public class EventReview : Review
{
    public int CompanyRate { get; set; }
    public int GoalComplianceRate { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;
}