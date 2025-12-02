using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameCatcher.Data;

namespace GameCatcher.Models;

public enum RequestStatus
{
    Pending,
    Accepted,
    Rejected,
}

public class FriendRequest
{
    [Key]
    public int RequestId;

    [Required]
    public string? SenderId { get; set; }

    [ForeignKey("SenderId")]
    public virtual ApplicationUser? Sender { get; set; }

    [Required]
    public string? ReceiverId { get; set; }

    [ForeignKey("ReceiverId")]
    public virtual ApplicationUser? Receiver { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
