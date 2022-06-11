using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasDiscriminator()
               .HasValue<Comment>("Comment")
               .HasValue<EventReview>(nameof(EventReview));
    }
}