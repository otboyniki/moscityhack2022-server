using Microsoft.AspNetCore.Identity;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class Interest : IEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public virtual ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
}