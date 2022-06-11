using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class HistoryActivityConfiguration : IEntityTypeConfiguration<HistoryActivity>
{
    public void Configure(EntityTypeBuilder<HistoryActivity> builder)
    {
        builder.HasKey(x => new { x.HistoryId, x.ActivityId });
    }
}