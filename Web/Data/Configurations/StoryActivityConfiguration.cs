using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class StoryActivityConfiguration : IEntityTypeConfiguration<StoryActivity>
{
    public void Configure(EntityTypeBuilder<StoryActivity> builder)
    {
        builder.HasKey(x => new { x.StoryId, x.ActivityId });
    }
}