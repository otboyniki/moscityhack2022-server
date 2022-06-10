using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Data.Entities;
using Web.Extensions;

#pragma warning disable CS8618

namespace Web.Data;

public class DataContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<Communication> Communications { get; set; }
    public DbSet<Verification> Verifications { get; set; }
    public DbSet<User> Users { get; set; }

    public DataContext(DbContextOptions options) : base(options) =>
        this.UseTimestamps();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<Enum>(x => x.HaveConversion<string>());

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
               .HasPostgresExtension("postgis");
    }
}