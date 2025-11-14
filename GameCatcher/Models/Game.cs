using System;

namespace GameCatcher.Models;

public class Game
{
    public long? GameId { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? ArtworkImageId { get; set; }
}
