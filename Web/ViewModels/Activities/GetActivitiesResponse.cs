namespace Web.ViewModels.Activities;

public class GetActivitiesResponse
{
    public ICollection<ActivityItem> ActivityItems { get; set; } = new List<ActivityItem>();
}

public class ActivityItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? IconPath { get; set; }
}