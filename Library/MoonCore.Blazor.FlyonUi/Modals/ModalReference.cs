using MoonCore.Blazor.FlyonUi.Common;

namespace MoonCore.Blazor.FlyonUi.Modals;

/// <summary>
/// Represents a reference to an active modal of the specified generic type
/// </summary>
/// <typeparam name="T">Type of the modal component</typeparam>
public sealed class ModalReference<T> : ModalReference where T : BaseModal
{
    /// <summary>
    /// Current instance of the defined component. Can be used to access e.g. public methods
    /// of the component
    /// </summary>
    public T Instance { get; set; }

    internal override object ComponentReference
    {
        get => Instance;
        set => Instance = (T)value;
    }
}

/// <summary>
/// Represents a type unspecific reference to an active modal
/// </summary>
public abstract class ModalReference
{
    /// <summary>
    /// Tailwind width class to define the size of the modal
    /// </summary>
    public string Size { get; set; }
    
    /// <summary>
    /// Toggles if clicking outside the modal (onto the backdrop) will hide the modal
    /// </summary>
    public bool AllowUnfocusHide { get; set; }
    
    internal ComponentOptions Options { get; set; }
    internal Type ComponentType { get; set; }
    internal abstract object ComponentReference { get; set; }
}