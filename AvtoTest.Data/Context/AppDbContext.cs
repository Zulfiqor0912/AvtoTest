using AvtoTest.Data.Entities;
using AvtoTest.Data.Entities.TestEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AvtoTest.Data.Context;

public class AppDbContext : IdentityDbContext<CustomUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<CustomUser> CustomUsers { get; set; }
    public DbSet<Result> Results { get; set; }
    //public DbSet<AnonymousUser> AnonymousUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
