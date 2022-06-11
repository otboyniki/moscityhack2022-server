using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Web.Data.Entities;
using Web.Extensions;
using File = Web.Data.Entities.File;

#pragma warning disable CS8618

namespace Web.Data;

public class DataContext : DbContext
{
    public DbSet<Communication> Communications { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Verification> Verifications { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }

    public DataContext(DbContextOptions options)
        : base(options) =>
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