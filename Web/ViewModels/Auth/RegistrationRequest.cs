namespace Web.ViewModels.Auth;

public abstract class RegistrationRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class VolunteerUserRegistrationRequest : RegistrationRequest { }

public class OrganizerUserRegistrationRequest : RegistrationRequest
{
    public string CompanyName { get; set; }
}