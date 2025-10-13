using MoonCore.Blazor.FlyonUi.Common;

namespace MoonCore.Blazor.FlyonUi.Toasts;

/// <summary>
/// Represents a reference to a specific active toast of a specific type of toast component
/// </summary>
/// <typeparam name="T">Type of the toast component</typeparam>
public class ToastReference<T> : ToastReference where T : BaseToast
{
    /// <summary>
    /// Current instance of the rendered component
    /// </summary>
    public T Instance { get; set; }

    internal override object ComponentReference
    {
        get => Instance;
        set => Instance = (T)value;
    }
}

/// <summary>
/// Type unspecific reference to an active toast
/// </summary>
public abstract class ToastReference
{
    internal ComponentOptions Options { get; set; }
    internal Type ComponentType { get; set; }
    internal abstract object ComponentReference { get; set; }
}