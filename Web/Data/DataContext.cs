using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Data.Entities;
using Web.Extensions;

#pragma warning disable CS8618

namespace Web.Data;

public class DataContext : IdentityDbContext<User, Role, string>
{
    public DbSet<Test> Tests { get; set; }

    public DataContext(DbContextOptions options)
        : base(options)
    {
        this.UseTimestamps();
    }
}