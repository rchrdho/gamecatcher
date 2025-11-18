using System;
using GameCatcher.Data;
using GameCatcher.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCatcher.DatabaseService;

public class ReviewService : IReviewService
{
    private readonly GameCatcherDbContext _dbContext;

    public ReviewService(GameCatcherDbContext gameCatcherDbContext)
    {
        _dbContext = gameCatcherDbContext;
    }

    public async Task AddReview(Review review)
    {
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveReview(int reviewId)
    {
        var review = await _dbContext.Reviews.FindAsync(reviewId);

        if (review != null)
        {
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<Review>> GetReviewsByGameId(long gameId)
    {
        return await _dbContext.Reviews.Where(r => r.GameId == gameId).ToListAsync();
    }
}
