using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class HistoryComment : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Text { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid HistoryId { get; set; }
    public History History { get; set; }

    public ICollection<HistoryCommentScore> HistoryCommentScores { get; set; } = new List<HistoryCommentScore>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}