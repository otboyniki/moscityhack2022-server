using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Web.Data.Entities;

namespace Web.ViewModels;

public class AddressDto
{
    public PointDto? PointLocation { get; set; }
    public string? StringLocation { get; set; }

    [NotMapped, JsonIgnore]
    public Address Address => new()
    {
        PointLocation = PointLocation?.Point,
        StringLocation = StringLocation
    };

    [NotMapped, JsonIgnore]
    public static Expression<Func<Address, AddressDto>> Projection => a => new AddressDto
    {
        PointLocation = a.PointLocation != null
            ? new PointDto(a.PointLocation)
            : null,
        StringLocation = a.StringLocation
    };
}