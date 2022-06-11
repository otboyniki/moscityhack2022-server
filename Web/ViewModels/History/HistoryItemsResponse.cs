using Web.Data.Entities;

namespace Web.ViewModels.History;

public class HistoryItemsResponse
{
    public HistoryItem? BigItem { get; set; }
    public ICollection<HistoryItem> Items { get; set; }
    public ICollection<ActivityFilterItem> ActivityFilters { get; set; }
}

public class ActivityFilterItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}

public class HistoryItem
{
    public string CompanyName { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public int Score { get; set; }
    public int CommentsCount { get; set; }
    public Guid? PreviewId { get; set; }
    public HistoryFormat Format { get; set; }
    public ICollection<HistoryItemsActivityItem> Activities { get; set; }
    public DateTime Date { get; set; }
}

public class HistoryItemsActivityItem
{
    public string Title { get; set; }
    public string IconPath { get; set; }
}