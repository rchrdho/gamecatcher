using System;
using Microsoft.AspNetCore.Identity;

namespace GameCatcher.Models;

// User Model with custom properties to the IdentityUser (AspUsers table)
// User will have profile picture url, list of reviews that they have written, list of friends
public class User : IdentityUser
{
    public string? ProfilePictureUrl { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<User>? Friends { get; set; }
}
