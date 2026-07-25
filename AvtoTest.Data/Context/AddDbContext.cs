using AvtoTest.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AvtoTest.Data.Context;

public class AddDbContext : IdentityDbContext<CustomUser>
{
    public AddDbContext(DbContextOptions<AddDbContext> options) : base(options)
    {
        
    }

    public DbSet<CustomUser> CustomUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
