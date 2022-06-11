using Web.Data.Enums;

namespace Web.ViewModels.Auth;

public class FastRegistrationRequestModel
{
    public CommunicationType Type { get; set; }
    public string Value { get; set; }
}