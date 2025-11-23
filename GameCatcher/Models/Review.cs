using System;
using System.ComponentModel.DataAnnotations;
using GameCatcher.Data;

namespace GameCatcher.Models;

public class Review
{
    [Key]
    public int ReviewId { get; set; }
    public long? GameId { get; set; }
    public string? Comment { get; set; }
    public double? Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    // User relation
    public ApplicationUser? Author { get; set; }
    public string? UserId { get; set; }
}
