using MoonCore.Blazor.FlyonUi.Common;

namespace MoonCore.Blazor.FlyonUi.Drawers;

/// <summary>
/// Represents a type unspecific reference to a drawer modal
/// </summary>
public abstract class DrawerReference
{
    /// <summary>
    /// Whether a click on the backdrop should close the drawer
    /// </summary>
    public bool AllowUnfocusHide { get; set; }
    
    /// <summary>
    /// Toggles if the drawer is visible. Used for the animation
    /// </summary>
    public bool IsVisible { get; set; }
    
    /// <summary>
    /// Direction in which the drawer should be opened
    /// </summary>
    public DrawerDirection Direction { get; set; }
    
    internal Type ComponentType { get; set; }
    internal abstract object ComponentReference { get; set; }
    internal ComponentOptions Options { get; set; }
}

/// <summary>
/// Represents a reference to an active drawer of the specified generic type
/// </summary>
/// <typeparam name="T">Type of the modal component</typeparam>
public class DrawerReference<T> : DrawerReference where T : DrawerBase
{
    /// <summary>
    /// Current instance of the defined component. Can be used to access e.g. public methods
    /// of the component
    /// </summary>
    public T Instance
    {
        get => (T)ComponentReference;
        set => ComponentReference = value;
    }
    
    internal override object ComponentReference { get; set; }
}