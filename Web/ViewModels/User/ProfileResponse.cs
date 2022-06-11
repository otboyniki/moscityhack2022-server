namespace Web.ViewModels.User;

public class ProfileResponse
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AvatarPath { get; set; }
    public ICollection<InterestModel> Interests { get; set; } = new List<InterestModel>();
}

public class InterestModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Enable { get; set; }
}