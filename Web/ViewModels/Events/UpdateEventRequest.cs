using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class UpdateEventRequest
{
    public Guid? PreviewId { get; set; }

    public Guid ActivityId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Terms { get; set; } = null!;

    public ICollection<AddressDto> Locations { get; set; } = new List<AddressDto>();

    public DateTimeRange? Recruitment { get; set; }
    public DateTimeRange Meeting { get; set; } = null!;
    public string? MeetingNote { get; set; }
}