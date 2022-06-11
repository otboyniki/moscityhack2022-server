namespace Web.Data.Entities;

public class HistoryView
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid HistoryId { get; set; }
    public History History { get; set; }
}