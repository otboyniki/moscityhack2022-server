using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Data.Entities;
using Web.Extensions;

#pragma warning disable CS8618

namespace Web.Data;

public class DataContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Test> Tests { get; set; }

    public DataContext(DbContextOptions options) : base(options) =>
        this.UseTimestamps();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<Enum>(x => x.HaveConversion<string>());

    protected override void OnModelCreating(ModelBuilder builder) =>
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
               .HasPostgresExtension("postgis");
}