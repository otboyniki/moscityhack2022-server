using System.ComponentModel.DataAnnotations.Schema;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

/// <summary>
///     Организация
/// </summary>
[Table("Companies")]
public class Company : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<User> Owners { get; set; } = new List<User>();
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}