using Microsoft.AspNetCore.Components.Authorization;

namespace MoonCore.Blazor.FlyonUI.Auth;

public abstract class AuthenticationStateManager : AuthenticationStateProvider
{
    public abstract Task Login();
    public abstract Task Logout();
}