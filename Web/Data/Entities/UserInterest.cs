using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class UserInterest
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid InterestId { get; set; }
    public Interest Interest { get; set; }
}