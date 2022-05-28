using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data.Entities;
using Web.Exceptions;
using static System.Environment;
using static System.String;

namespace Web.Data;

public class Seeder
{
    public static void Migrate(IServiceProvider provider)
    {
        var context = provider.GetRequiredService<DataContext>();
        var logger = provider.GetRequiredService<ILogger<Seeder>>();

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

    public static async Task Seed(IServiceProvider provider)
    {
        var roleManager = provider.GetRequiredService<RoleManager<Role>>();

        foreach (var role in Roles.Select(x => roleManager.FindByNameAsync(x).Result == null ? x : null)
                                  .Where(x => x != null))
        {
            var result = await roleManager.CreateAsync(new Role { Name = role });
            if (!result.Succeeded)
            {
                var identityError = result.Errors.First();
                throw new RestException(identityError.Description, identityError.Code, HttpStatusCode.UnprocessableEntity);
            }
        }
    }

    private static readonly string[] Roles =
    {
        "admin",
        "moderator"
    };
}