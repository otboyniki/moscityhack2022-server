using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class User : IdentityUser<Guid>, IEntity, IHasTimestamps
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? Birthday { get; set; }

//    public Point? Location { get; set; }

    public Guid? AvatarId { get; set; }
    public File? Avatar { get; set; } = null!;

    public virtual ICollection<Communication> Communications { get; set; } = new List<Communication>();
    public virtual ICollection<Participation> Participants { get; set; } = new List<Participation>();
    public virtual ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}