using System;
using System.Text.Json.Serialization;

namespace GameCatcher.Models;

public class PeakGames
{
    public int Id { get; set; }

    [JsonPropertyName("game_id")]
    public int GameId { get; set; }

    public int PopularityType { get; set; }
}
