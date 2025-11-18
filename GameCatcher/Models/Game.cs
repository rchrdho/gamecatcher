using System;
using System.ComponentModel.DataAnnotations;
using IGDB;

namespace GameCatcher.Models;

public class Game
{
    [Key]
    public long? GameId { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? ArtworkImageId { get; set; }
    public DateTimeOffset? ReleaseDate { get; set; }
    public string? Genre { get; set; }
    public string? Platform { get; set; }
    public double? Rating { get; set; }
}
