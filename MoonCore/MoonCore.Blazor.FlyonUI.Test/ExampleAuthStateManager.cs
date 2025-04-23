using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MoonCore.Blazor.FlyonUI.Auth;

namespace MoonCore.Blazor.FlyonUI.Test;

public class ExampleAuthStateManager : AuthenticationStateManager
{
    private readonly NavigationManager Navigation;

    public ExampleAuthStateManager(NavigationManager navigation)
    {
        Navigation = navigation;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        AuthenticationState authState;

        var random = new Random();
        //var loggedIn = random.Next(0, 2) == 1;
        var loggedIn = true;
        
        if (loggedIn)
        {
            authState = new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        [
                            new Claim("username", "demouser"),
                            new Claim("email", "demo@email.xyz")
                        ],
                        "auth"
                    )
                )
            );
        }
        else
        {
            authState = new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()
                )
            );
        }

        //await Task.Delay(TimeSpan.FromSeconds(1));

        return authState;
    }

    public override async Task Login()
    {
        await Task.Delay(1000);
        Navigation.NavigateTo(Navigation.Uri, true);
    }

    public override Task Logout()
    {
        throw new NotImplementedException();
    }
}