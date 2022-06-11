using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class ReviewScore : IEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    public bool Positive { get; set; }
}