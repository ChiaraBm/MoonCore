using Microsoft.AspNetCore.Components;

namespace MoonCore.Blazor.FlyonUI.Modals;

public class ModalItem
{
    public string Size { get; set; }
    public RenderFragment Component { get; set; }
    public bool AllowUnfocusHide { get; set; }
    public bool IsVisible { get; set; }
}