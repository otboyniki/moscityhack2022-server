using Microsoft.EntityFrameworkCore;

namespace Web.Data.Entities;

[Owned]
public class IntRange
{
    public int From { get; set; }
    public int To { get; set; }
}