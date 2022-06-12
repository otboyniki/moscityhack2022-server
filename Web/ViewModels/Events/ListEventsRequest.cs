
namespace Web.ViewModels.Events;

public class ListEventsRequest : IPaginationRequest
{
    public int Page { get; set; }
    public int? Limit { get; set; }

    public bool? IsArchived { get; set; }
    public bool? IsOnline { get; set; }

    public ICollection<Guid>? Activities { get; set; }

    public DateTime? Since { get; set; }
    public DateTime? Until { get; set; }

    public string? StringLocation { get; set; }

    public int? FromAge { get; set; }
    public int? ToAge { get; set; }
}