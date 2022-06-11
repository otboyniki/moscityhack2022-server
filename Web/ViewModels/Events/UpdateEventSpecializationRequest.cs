namespace Web.ViewModels.Events;

public class UpdateEventSpecializationRequest
{
    public string Title { get; set; } = null!;
    public string? Requirements { get; set; }
    public string Description { get; set; } = null!;

    public int MinVolunteersNumber { get; set; }
    public int MaxVolunteersNumber { get; set; }
    public bool IsRegisteredVolunteersNeeded { get; set; }
}