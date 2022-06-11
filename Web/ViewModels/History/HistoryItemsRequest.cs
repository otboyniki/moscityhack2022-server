namespace Web.ViewModels.History;

public class HistoryItemsRequest
{
    public HistoryFilter Filters { get; set; } = new();
    public HistorySort Sort { get; set; } = new();
}

public class HistoryFilter
{
    public ICollection<Guid> InterestIds { get; set; } = new List<Guid>();

    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    public int? FromScore { get; set; }
    public int? ToScore { get; set; }

    public int Skip { get; set; }
    public int Take { get; set; } = 30;
}

public class HistorySort
{
    public bool IsAscending { get; set; }
    public HistorySortValue Value { get; set; }
}

public enum HistorySortValue
{
    Score = 1,
    Views = 2,
}