using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class EventSummaryDto
{
    public Guid EventId { get; set; }

    public string Title { get; set; } = null!;
    public string StringLocation { get; set; } = null!;
    public DateTimeRange Meeting { get; set; } = null!;
}