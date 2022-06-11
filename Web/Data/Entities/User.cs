using Microsoft.AspNetCore.Identity;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

public abstract class User : IdentityUser<Guid>, IEntity, IHasTimestamps
{
    public string Discriminator { get; private set; } = null!;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? Birthday { get; set; }

    public Address? Address { get; set; }

    public Guid? AvatarId { get; set; }
    public File? Avatar { get; set; }

    public ICollection<Communication> Communications { get; set; } = new List<Communication>();
    public ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
    public ICollection<HistoryView> HistoryViews { get; set; } = new List<HistoryView>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class VolunteerUser : User
{
    public virtual ICollection<Participation> Participants { get; set; } = new List<Participation>();
}

public class OrganizerUser : User
{
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}