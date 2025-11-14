using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace GameCatcher.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // non-authenticated user
        // var user = new ClaimsPrincipal(new ClaimsIdentity());

        // authenticated user
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "John") };
        var identity = new ClaimsIdentity(claims, "ANY");
        var user = new ClaimsPrincipal(identity);

        return Task.FromResult(new AuthenticationState(user));
    }
}
