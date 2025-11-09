using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Company.Frontend.AuthenticationProviders;

public class AuthenticationProviderTest : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonimous = new ClaimsIdentity();
        var user = new ClaimsIdentity(authenticationType: "test");
        var admin = new ClaimsIdentity(
    [
        new("FirstName", "Andrea"),
        new("LastName", "Higuita"),
        new(ClaimTypes.Name, "andrea@yopmail.com"),
        new(ClaimTypes.Role, "Admin")
    ],
    authenticationType: "test");

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(admin)));
    }
}