namespace MoonCore.Blazor.FlyonUI.Modals;

public class ModalService
{
    private ModalLauncher Launcher;

    public void UpdateLauncher(ModalLauncher launcher)
        => Launcher = launcher;

    public async Task<ModalItem> Launch<T>(Action<Dictionary<string, object>>? onConfigure = null,
        string size = "modal-dialog-lg", bool allowUnfocusHide = false, bool animate = true) where T : BaseModal
        => await Launcher.Launch<T>(onConfigure, size, allowUnfocusHide, animate);
}