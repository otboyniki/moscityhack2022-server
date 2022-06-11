using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(x => x.Address, b =>
        {
            b.Property(x => x.PointLocation)
             .HasColumnType("geography (point)");
        });

        builder.HasDiscriminator()
               .HasValue<VolunteerUser>("VolunteerUser")
               .HasValue<OrganizerUser>("OrganizerUser");
    }
}