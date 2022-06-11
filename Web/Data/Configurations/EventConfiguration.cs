using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.OwnsMany(x => x.Locations, b =>
        {
            b.Property(x => x.PointLocation)
             .HasColumnType("geography (point)");
        });
    }
}