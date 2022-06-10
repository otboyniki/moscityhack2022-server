using Microsoft.AspNetCore.Identity;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class User : IdentityUser<Guid>, IEntity, IHasTimestamps
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Patronymic { get; set; }

    public virtual ICollection<Communication> Communications { get; set; } = new List<Communication>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}