using System;

namespace GameCatcher.Models;

public class GameDetails
{
    public long? GameId { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public Cover? Cover { get; set; }

    public string? ArtworkUrl { get; set; }
}
