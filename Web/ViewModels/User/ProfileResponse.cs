using Web.Data.Entities;

namespace Web.ViewModels.User;

public class ProfileResponse
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Guid? AvatarId { get; set; }
    public AddressDto? Location { get; set; }
    public Gender? Gender { get; set; }
    public string[] SocialNetworks { get; set; }
    public string[] Languages { get; set; }
    public string? Education { get; set; }
    public ProfileType ProfileType { get; set; }
    public ICollection<InterestModel> Interests { get; set; } = new List<InterestModel>();
}

public class InterestModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Enable { get; set; }
}

public enum ProfileType
{
    Organizer = 1,
    Volunteer = 2,
}