using Microsoft.Extensions.DependencyInjection;
using MoonCore.Blazor.FlyonUI.Alerts;
using MoonCore.Blazor.FlyonUI.Modals;

namespace MoonCore.Blazor.FlyonUI;

public static class FlyonUiExtensions
{
    public static void AddFlyonUI(this IServiceCollection collection)
    {
        collection.AddScoped<ModalService>();
        collection.AddScoped<AlertService>();
    }
}