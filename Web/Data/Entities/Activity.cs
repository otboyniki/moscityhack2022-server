using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class Activity : IEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public Guid? IconId { get; set; }
    public File? Icon { get; set; }

    public virtual ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
}