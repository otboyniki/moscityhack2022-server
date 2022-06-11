namespace Web.ViewModels.Story;

public class InfoNewStoryResponse
{
    public ICollection<InfoNewStoryActivityItem> Activities { get; set; } = new List<InfoNewStoryActivityItem>();
}

public class InfoNewStoryActivityItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}