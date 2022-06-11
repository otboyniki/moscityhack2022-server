using Web.Data.Entities;

namespace Web.ViewModels.Story;

public class StoryItemsResponse
{
    public ICollection<StoryItem> Items { get; set; }
}

public class StoryItem
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public int Score { get; set; }
    public int CommentsCount { get; set; }
    public int ViewsCount { get; set; }
    public Guid? PreviewId { get; set; }
    public StoryFormat Format { get; set; }
    public ICollection<StoryItemsActivityItem> Activities { get; set; }
    public DateTime Date { get; set; }
}

public class StoryItemsActivityItem
{
    public string Title { get; set; }
    public Guid? IconId { get; set; }
}