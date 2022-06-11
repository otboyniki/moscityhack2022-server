using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class StoryScoreConfiguration : IEntityTypeConfiguration<StoryScore>
{
    public void Configure(EntityTypeBuilder<StoryScore> builder)
    {
        builder.HasKey(x => new { x.UserId, x.StoryId });
    }
}