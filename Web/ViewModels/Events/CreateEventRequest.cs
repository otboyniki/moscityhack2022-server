namespace Web.ViewModels.Events;

public class CreateEventRequest : UpdateEventRequest
{
    public UpdateEventSpecializationRequest[]? Specializations { get; set; }
}