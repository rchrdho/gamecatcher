using System;
using GameCatcher.Models;

namespace GameCatcher.DatabaseService;

public interface IReviewService
{
    Task AddReview(Review review);

    Task RemoveReview(int reviewId);

    Task<List<Review>> GetReviewsByGameId(long gameId);
}
