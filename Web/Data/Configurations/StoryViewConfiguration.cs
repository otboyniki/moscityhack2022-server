using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class StoryViewConfiguration : IEntityTypeConfiguration<StoryView>
{
    public void Configure(EntityTypeBuilder<StoryView> builder)
    {
        builder.HasKey(x => new { x.UserId, StoryId = x.StoryId });
    }
}