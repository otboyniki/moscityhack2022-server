using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Web.Data.Entities;

[Owned]
public class Address
{
    public Point? PointLocation { get; set; }
    public string? StringLocation { get; set; }
}