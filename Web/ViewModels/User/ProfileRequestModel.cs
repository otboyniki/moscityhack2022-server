namespace Web.ViewModels.User;

public class ProfileRequestModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public IFormFile? Avatar { get; set; }
    public ICollection<Guid> InterestIds { get; set; } = new List<Guid>();
}