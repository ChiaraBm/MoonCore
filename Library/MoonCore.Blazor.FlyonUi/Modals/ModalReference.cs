using MoonCore.Blazor.FlyonUi.Common;

namespace MoonCore.Blazor.FlyonUi.Modals;

public sealed class ModalReference<T> : ModalReference where T : BaseModal
{
    public T Instance { get; set; }

    internal override object ComponentReference
    {
        get => Instance;
        set => Instance = (T)value;
    }
}

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