using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoonCore.Blazor.FlyonUI;
using MoonCore.Blazor.FlyonUI.Auth;
using MoonCore.Blazor.FlyonUI.Test;
using MoonCore.Blazor.FlyonUI.Test.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateManager<ExampleAuthStateManager>();

builder.Services.AddFlyonUI();

await builder.Build().RunAsync();