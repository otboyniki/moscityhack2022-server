namespace Web.ViewModels.Events;

public class CreateEventRequest : UpdateEventRequest
{
    public UpdateEventSpecializationRequest? Specialization { get; set; }
}