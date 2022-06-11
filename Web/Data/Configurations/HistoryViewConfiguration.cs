using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Data.Entities;

namespace Web.Data.Configurations;

public class HistoryViewConfiguration : IEntityTypeConfiguration<HistoryView>
{
    public void Configure(EntityTypeBuilder<HistoryView> builder)
    {
        builder.HasKey(x => new { x.UserId, x.HistoryId });
    }
}