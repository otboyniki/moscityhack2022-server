namespace Web.ViewModels.Events;

public class CreateEventRequest : UpdateEventRequest
{
    public ICollection<UpdateEventSpecializationRequest> Specializations { get; set; } = new List<UpdateEventSpecializationRequest>();
}