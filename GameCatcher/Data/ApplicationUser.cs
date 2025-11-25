using System.ComponentModel.DataAnnotations.Schema;
using GameCatcher.Models;
using Microsoft.AspNetCore.Identity;

namespace GameCatcher.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? ProfilePictureUrl { get; set; }
    public List<Review>? Reviews { get; set; }
    public virtual ICollection<ApplicationUser>? Friends { get; set; }
    public virtual ICollection<ApplicationUser>? FriendOf { get; set; }
}
