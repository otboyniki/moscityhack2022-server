using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using static System.Environment;
using static System.String;

namespace Web.Data;

public class Seeder
{
    public static void Migrate(IServiceProvider provider)
    {
        var serviceProvider = provider.CreateScope().ServiceProvider;
        var context = serviceProvider.GetRequiredService<DataContext>();
        var logger = serviceProvider.GetRequiredService<ILogger<Seeder>>();

        var migrations = context.Database
                                .GetPendingMigrations()
                                .ToList();

        if (migrations.Count > 0)
        {
            var sw = new Stopwatch();

            sw.Start();
            context.Database.Migrate();
            sw.Stop();

            logger.LogInformation(Join(NewLine,
                migrations.Select(x => $"migrate: {x}")
                          .Append($"Database migrated successfully in {sw.ElapsedMilliseconds} ms")));
        }
        else
        {
            logger.LogInformation("Nothing to migrate");
        }
    }
}