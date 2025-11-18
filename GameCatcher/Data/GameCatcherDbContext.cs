using GameCatcher.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCatcher.Data;

public class GameCatcherDbContext(DbContextOptions<GameCatcherDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Game> Games { get; set; }

    public DbSet<Review> Reviews { get; set; }
}
