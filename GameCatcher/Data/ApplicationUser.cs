using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using GameCatcher.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace GameCatcher.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public required string? DisplayName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public List<Review>? Reviews { get; set; }
    public virtual ICollection<ApplicationUser>? Friends { get; set; }
    public virtual ICollection<ApplicationUser>? FriendOf { get; set; }
}

public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public CustomUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor
    )
        : base(userManager, optionsAccessor) { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("FactoryTest", "It Works!"));

        if (!string.IsNullOrEmpty(user.DisplayName))
        {
            identity.AddClaim(new Claim("DisplayName", user.DisplayName));
        }

        if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
        {
            identity.AddClaim(new Claim("ProfilePictureUrl", user.ProfilePictureUrl));
        }

        return identity;
    }
}
