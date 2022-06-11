using Microsoft.EntityFrameworkCore;

namespace Web.Data.Entities;

[Owned]
public class DateTimeRange
{
    public DateTime Since { get; set; }
    public DateTime Until { get; set; }
}