using Web.Data.Interfaces;

namespace Web.Data.Entities;

public abstract class Review : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Text { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<ReviewScore> ReviewScores { get; set; } = new List<ReviewScore>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Comment : Review
{
    public Guid HistoryId { get; set; }
    public History History { get; set; }
}