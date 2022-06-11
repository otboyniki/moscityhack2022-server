using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace Web.ViewModels;

public class PointDto
{
    /// <summary>
    ///     Широта (в градусах)
    /// </summary>
    public double Lat { get; set; }

    /// <summary>
    ///     Долгота (в градусах)
    /// </summary>
    public double Lon { get; set; }

    [NotMapped, JsonIgnore]
    public Point Point => new(Lon, Lat);

    public PointDto()
    {
    }

    public PointDto(Point point)
    {
        Lon = point.X;
        Lat = point.Y;
    }
}