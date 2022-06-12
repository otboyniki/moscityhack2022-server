using Web.Data.Entities;

namespace Web.ViewModels.Events;

public class UpdateEventSpecializationRequest
{
    public string Title { get; set; } = null!;
    public string? Requirements { get; set; }
    public string Description { get; set; } = null!;

    public bool IsOnline { get; set; }
    public IntRange? Ages { get; set; }

    public int MinVolunteersNumber { get; set; }
    public int MaxVolunteersNumber { get; set; }
    public bool IsRegisteredVolunteersNeeded { get; set; }
}