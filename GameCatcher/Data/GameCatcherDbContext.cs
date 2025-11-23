using GameCatcher.Components;
using GameCatcher.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCatcher.Data;

public class GameCatcherDbContext : IdentityDbContext<ApplicationUser>
{
    public GameCatcherDbContext(DbContextOptions<GameCatcherDbContext> options)
        : base(options) { }

    public DbSet<Game> Games { get; set; }

    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<ApplicationUser>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }
}
