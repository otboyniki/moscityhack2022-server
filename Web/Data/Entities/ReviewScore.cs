using Web.Data.Interfaces;

namespace Web.Data.Entities;

public class ReviewScore : IEntity
{
    public Guid Id { get; set; }

    public Guid ReviewId { get; set; }
    public Review Review { get; set; }

    public bool Positive { get; set; }
}