namespace Web.Data.Entities;

public class HistoryActivity
{
    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }

    public Guid HistoryId { get; set; }
    public History History { get; set; }
}