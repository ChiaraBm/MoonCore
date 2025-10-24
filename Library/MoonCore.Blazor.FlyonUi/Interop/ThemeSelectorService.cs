using Microsoft.JSInterop;

namespace MoonCore.Blazor.FlyonUi.Interop;

/// <summary>
/// Provides access to the interop methods for selecting themes for FlyonUi
/// </summary>
public class ThemeSelectorService
{
    private readonly IJSRuntime JsRuntime;

    /// <summary>
    /// Creates a new instance of the wrapper class for the theme selector interop
    /// </summary>
    /// <param name="jsRuntime"></param>
    public ThemeSelectorService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    /// <summary>
    /// Set the theme to the one with the provided name
    /// </summary>
    /// <param name="themeName">Name of the theme</param>
    public async Task SetAsync(string themeName)
    {
        await JsRuntime.InvokeVoidAsync("moonCore.themeSelector.set", themeName);
    }
}