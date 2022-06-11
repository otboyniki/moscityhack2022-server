namespace Web.Data.Entities;

public class HistoryCommentScore
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid HistoryCommentId { get; set; }
    public HistoryComment HistoryComment { get; set; }

    public bool Positive { get; set; }
}