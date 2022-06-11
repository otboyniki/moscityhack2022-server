using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class HistoryScoreConfiguration : IEntityTypeConfiguration<HistoryScore>
{
    public void Configure(EntityTypeBuilder<HistoryScore> builder)
    {
        builder.HasKey(x => new { x.UserId, x.HistoryId });
    }
}