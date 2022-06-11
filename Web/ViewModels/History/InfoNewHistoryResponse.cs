namespace Web.ViewModels.History;

public class InfoNewHistoryResponse
{
    public ICollection<InfoNewHistoryActivityItem> Activities { get; set; } = new List<InfoNewHistoryActivityItem>();
}

public class InfoNewHistoryActivityItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}