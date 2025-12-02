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
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // one-to-many relationship for Reviews
        // each user can have many Reviews, each Review has only one Author
        // with foreign key UserId in Review
        // cascade delete Reviews when User is deleted
        modelBuilder
            .Entity<ApplicationUser>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.Author)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // many-to-many relationship for Friends
        // each user has many Friends, each Friend can have many Users
        // using a join table "UserFriends"
        modelBuilder
            .Entity<ApplicationUser>()
            .HasMany(u => u.Friends)
            .WithMany(u => u.FriendOf)
            .UsingEntity(join => join.ToTable("UserFriends"));

        // declaring primary key
        modelBuilder.Entity<FriendRequest>().HasKey(r => r.RequestId);
        modelBuilder.Entity<Notification>().HasKey(n => n.NotificationId);

        modelBuilder
            .Entity<FriendRequest>()
            .HasIndex(r => new { r.SenderId, r.ReceiverId })
            .IsUnique()
            .HasFilter("[Status] = 0");

        modelBuilder
            .Entity<FriendRequest>()
            .HasOne(r => r.Sender)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder
            .Entity<FriendRequest>()
            .HasOne(r => r.Receiver)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
