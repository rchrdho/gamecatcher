using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCatcher.Data;
using GameCatcher.DatabaseService;
using GameCatcher.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GameCatcher.Tests;

public class ReviewServiceTests
{
    private GameCatcherDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<GameCatcherDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
            .Options;
        return new GameCatcherDbContext(options);
    }

    [Fact]
    public async Task AddReview_ShouldAddReviewToDatabase()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ReviewService(context);
        var review = new Review
        {
            GameId = 1,
            UserId = "user1",
            Comment = "Great game!",
            Rating = 5,
            CreatedAt = DateTime.Now,
        };

        // Act
        await service.AddReview(review);

        // Assert
        var savedReview = await context.Reviews.FirstOrDefaultAsync();
        Assert.NotNull(savedReview);
        Assert.Equal("Great game!", savedReview.Comment);
        Assert.Equal(5, savedReview.Rating);
    }

    [Fact]
    public async Task RemoveReview_ShouldRemoveReviewFromDatabase()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ReviewService(context);
        var review = new Review
        {
            ReviewId = 1,
            GameId = 1,
            UserId = "user1",
        };
        context.Reviews.Add(review);
        await context.SaveChangesAsync();

        // Act
        await service.RemoveReview(1);

        // Assert
        var deletedReview = await context.Reviews.FindAsync(1);
        Assert.Null(deletedReview);
    }

    [Fact]
    public async Task GetReviewsByGameId_ShouldReturnOnlyReviewsForSpecificGame()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ReviewService(context);
        context.Reviews.AddRange(
            new Review
            {
                ReviewId = 1,
                GameId = 100,
                Comment = "Game 100 Review",
            },
            new Review
            {
                ReviewId = 2,
                GameId = 100,
                Comment = "Another Game 100 Review",
            },
            new Review
            {
                ReviewId = 3,
                GameId = 200,
                Comment = "Game 200 Review",
            }
        );
        await context.SaveChangesAsync();

        // Act
        var results = await service.GetReviewsByGameId(100);

        // Assert
        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.Equal(100, r.GameId));
    }

    [Fact]
    public async Task GetReviewsByUserId_ShouldReturnOnlyReviewsForSpecificUser()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new ReviewService(context);
        context.Reviews.AddRange(
            new Review
            {
                ReviewId = 1,
                UserId = "userA",
                GameId = 1,
            },
            new Review
            {
                ReviewId = 2,
                UserId = "userB",
                GameId = 1,
            }
        );
        await context.SaveChangesAsync();

        // Act
        var results = await service.GetReviewsByUserId("userA");

        // Assert
        Assert.Single(results);
        Assert.Equal("userA", results[0].UserId);
    }
}
