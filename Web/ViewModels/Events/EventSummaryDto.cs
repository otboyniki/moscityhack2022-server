using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class EventSummaryDto
{
    public Guid EventId { get; set; }

    public Guid? PreviewId { get; set; }
    public string Title { get; set; } = null!;
    public ICollection<AddressDto> Locations { get; set; } = null!;
    public DateTimeRange Meeting { get; set; } = null!;
}