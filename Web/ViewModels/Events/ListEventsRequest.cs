using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class ListEventsRequest : IPaginationRequest
{
    public int Page { get; set; }
    public int? Limit { get; set; }

    public bool WithArchived { get; set; }
    public ICollection<Guid>? Activities { get; set; }
    public DateTime? Since { get; set; }
    public DateTime? Until { get; set; }
}