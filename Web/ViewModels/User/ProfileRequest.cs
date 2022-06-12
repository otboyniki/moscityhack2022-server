using Web.Data.Entities;

namespace Web.ViewModels.User;

public class ProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public AddressDto? Location { get; set; }
    public DateTime? Birthday { get; set; }
    public Guid? AvatarId { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string[] SocialNetworks { get; set; }
    public string[] Languages { get; set; }
    public string? Education { get; set; }
    public ICollection<Guid> ActivityIds { get; set; } = new List<Guid>();
}