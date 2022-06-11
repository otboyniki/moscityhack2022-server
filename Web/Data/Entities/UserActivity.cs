namespace Web.Data.Entities;

public class UserActivity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }
}