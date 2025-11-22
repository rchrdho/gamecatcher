using GameCatcher.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCatcher.Data;

// public class GameCatcherDbContext(DbContextOptions<GameCatcherDbContext> options)
//     : IdentityDbContext<ApplicationUser>(options)
// {
//     public DbSet<Game> Games { get; set; }

//     public DbSet<Review> Reviews { get; set; }
// }

public class GameCatcherDbContext : IdentityDbContext<User>
{
    public GameCatcherDbContext(DbContextOptions<GameCatcherDbContext> options)
        : base(options) { }

    public DbSet<Game> Games { get; set; }

    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
