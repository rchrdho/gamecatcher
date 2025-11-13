using System;

namespace GameCatcher.Models
{
    public class Review
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}