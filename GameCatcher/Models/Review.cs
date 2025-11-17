using System;

namespace GameCatcher.Models;

public class Review
{
    public long? GameId { get; set; }
    public string? UserId { get; set; }
    public string? Comment { get; set; }
    public double? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
