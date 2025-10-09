using MoonCore.Blazor.FlyonUi.Toasts.Components;

namespace MoonCore.Blazor.FlyonUi.Toasts;

public class ToastReference<T> : ToastReference where T : BaseToast
{
    public ToastReference(Type componentType, Action<T>? configureAction)
    {
        ComponentType = componentType;

        if (configureAction != null)
            ConfigureAction = () => configureAction.Invoke(Instance);
    }
    
    public T Instance { get; set; }

    internal override BaseToast Component
    {
        get => Instance;
        set => Instance = (T)value;
    }
}

public abstract class ToastReference
{
    internal Type ComponentType { get; set; }
    internal abstract BaseToast Component { get; set; }
    internal Action? ConfigureAction { get; set; }
}