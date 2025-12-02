using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameCatcher.Data;

namespace GameCatcher.Models;

public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [Required]
    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    public string? Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
