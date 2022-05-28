using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Web.Data.Interfaces;

namespace Web.Extensions;

public static class DbContextExtensions
{
    #region Timestamps

    public static DbContext UseTimestamps(this DbContext context)
    {
        context.SavingChanges += OnSavingChangesUpdateTimestamps;
        return context;
    }

    private static void OnSavingChangesUpdateTimestamps(object? sender,
                                                        SavingChangesEventArgs savingChangesEventArgs)
    {
        if (sender is not DbContext context) return;
        var now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted &&
                entry.Entity is ISoftDeletes { DeletedAt: null } softDeletesEntity)
            {
                entry.State = EntityState.Modified;
                softDeletesEntity.DeletedAt = now;
            }

            if (entry.Entity is IHasTimestamps entityWithTimestamps)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entityWithTimestamps.CreatedAt = now;
                        entityWithTimestamps.UpdatedAt = now;
                        break;

                    case EntityState.Modified:
                        var createdAtProperty = entry.Property(nameof(IHasTimestamps.CreatedAt));

                        if (createdAtProperty.IsModified &&
                            (DateTime)createdAtProperty.CurrentValue! != (DateTime)createdAtProperty.OriginalValue!)
                        {
                            context.GetService<ILogger<DbContext>>()
                                   .LogWarning(
                                       "Attempt to update CreatedAt timestamp for entity \"{0}\"\n" +
                                       "Entity ID: {1}\n" +
                                       "Original Timestamp: {2}\n" +
                                       "Current Timestamp:  {3}\n" +
                                       "Changes will be reverted",
                                       entry.Entity.GetType().FullName,
                                       entry.Property(nameof(IEntity.Id)).CurrentValue,
                                       createdAtProperty.OriginalValue,
                                       entityWithTimestamps.CreatedAt);

                            entityWithTimestamps.CreatedAt = (DateTime)createdAtProperty.OriginalValue;
                        }

                        entityWithTimestamps.UpdatedAt = now;
                        break;
                }
            }
        }
    }

    #endregion
}