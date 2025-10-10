namespace MoonCore.Blazor.FlyonUi.Modals;

public class ModalReference<T> : ModalReference where T : BaseModal
{
    public ModalReference(Type componentType, Action<T>? configureAction)
    {
        ComponentType = componentType;

        if (configureAction != null)
            ConfigureAction = () => configureAction.Invoke(Instance);
    }
    
    public T Instance { get; set; }

    internal override BaseModal Component
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
    
    internal Type ComponentType { get; set; }
    internal abstract BaseModal Component { get; set; }
    internal Action? ConfigureAction { get; set; }
}