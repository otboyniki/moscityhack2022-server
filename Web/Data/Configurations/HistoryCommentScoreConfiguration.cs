using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class HistoryCommentScoreConfiguration : IEntityTypeConfiguration<HistoryCommentScore>
{
    public void Configure(EntityTypeBuilder<HistoryCommentScore> builder)
    {
        builder.HasKey(x => new { x.UserId, x.HistoryCommentId });
    }
}