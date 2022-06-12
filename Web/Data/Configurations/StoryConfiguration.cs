using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder.HasDiscriminator()
               .HasValue<CompanyStory>(nameof(CompanyStory))
               .HasValue<UserStory>(nameof(UserStory));
    }
}