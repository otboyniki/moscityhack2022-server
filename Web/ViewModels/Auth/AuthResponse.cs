namespace Web.ViewModels.Auth;

public class AuthResponse
{
    public Guid Id { get; set; }

    public AuthResponse(Guid id) => Id = id;
}