using Web.Data.Interfaces;

namespace Web.ViewModels;

public class Entity
{
    public Guid Id { get; init; }

    public static Entity From(IEntity entity) =>
        new()
        {
            Id = entity.Id
        };
}