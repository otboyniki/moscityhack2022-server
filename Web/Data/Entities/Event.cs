using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using Web.Data.Interfaces;

namespace Web.Data.Entities;

[Table("Events")]
public class Event : IEntity, IHasTimestamps
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public Point Location { get; set; } = null!;
    public string Kind { get; set; } = null!;

    public DateTime Since { get; set; }
    public DateTime Until { get; set; }

    public int MinVolunteersNumber { get; set; }
    public int MaxVolunteersNumber { get; set; }

    public bool IsRegisteredVolunteersNeeded { get; set; }

    // todo: специальности

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}