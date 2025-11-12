using System;
using System.Text.Json.Serialization;

namespace GameCatcher.Models;

public class PeakGames
{
    [JsonPropertyName("game_id")]
    public long? GameId { get; set; }
}
