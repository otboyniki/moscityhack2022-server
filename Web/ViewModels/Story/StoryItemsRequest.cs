namespace Web.ViewModels.Story;

public class StoryItemsRequest
{
    public StoryFilter Filters { get; set; } = new();
    public StorySort Sort { get; set; } = new();
}

public class StoryFilter
{
    public ICollection<Guid> InterestIds { get; set; } = new List<Guid>();

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public int? FromScore { get; set; }
    public int? ToScore { get; set; }

    public int Skip { get; set; }
    public int Take { get; set; } = 30;
}

public class StorySort
{
    public bool IsAscending { get; set; }
    public StorySortValue Value { get; set; } = StorySortValue.Score;
}

public enum StorySortValue
{
    Score = 1,
    Views = 2,
}