using Web.Data.Enums;

namespace Web.ViewModels.Auth;

public class FastRegistrationRequest
{
    public string Name { get; set; }
    public CommunicationType Type { get; set; }
    public string Value { get; set; }
}