using System.ComponentModel.DataAnnotations.Schema;
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
    public Gender? Gender { get; set; }
    public string[]? SocialNetworks { get; set; }
    public string[]? Languages { get; set; }
    public string? Education { get; set; }

    public Address? Address { get; set; }

    public Guid? AvatarId { get; set; }
    public File? Avatar { get; set; }

    public ICollection<UserStory> Stories { get; set; } = new List<UserStory>();
    public ICollection<Communication> Communications { get; set; } = new List<Communication>();
    public ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
    public ICollection<StoryView> StoryViews { get; set; } = new List<StoryView>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
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

public enum Gender
{
    Male = 1,
    Female = 2,
}