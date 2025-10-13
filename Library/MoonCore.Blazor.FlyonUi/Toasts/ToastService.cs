using MoonCore.Blazor.FlyonUi.Common;
using MoonCore.Blazor.FlyonUi.Toasts.Components;

namespace MoonCore.Blazor.FlyonUi.Toasts;

/// <summary>
/// Provides access to the toast launcher instance in order to launch and close toasts using the launcher
/// </summary>
public class ToastService
{
    private ToastLauncher ToastLauncher;

    /// <summary>
    /// Launches a success toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public Task SuccessAsync(string title, string text = "")
        => LaunchStatusAsync<SuccessToast>(title, text);

    /// <summary>
    /// Launches an info toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public Task InfoAsync(string title, string text = "")
        => LaunchStatusAsync<InfoToast>(title, text);

    /// <summary>
    /// Launches a warning toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public Task WarningAsync(string title, string text = "")
        => LaunchStatusAsync<WarningToast>(title, text);

    /// <summary>
    /// Launches an error toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public Task ErrorAsync(string title, string text = "")
        => LaunchStatusAsync<ErrorToast>(title, text);

    private async Task LaunchStatusAsync<T>(string title, string text) where T : BaseToast
    {
        await ToastLauncher.LaunchAsync<T>(options =>
        {
            options.Add("Title", title);
            options.Add("Text", text);
        });
    }

    /// <summary>
    /// Launches a progress toast which is active as long as the defined work callback is running.
    /// You can set the text representing the status withing this toast using <see cref="ProgressToast.UpdateTextAsync"/> method
    /// </summary>
    /// <param name="title">Title of the progress toast</param>
    /// <param name="text">Default status text which is shown until the <see cref="ProgressToast.UpdateTextAsync"/> method is called</param>
    /// <param name="work">Callback which is invoked when the toast appears and hides the toast when it returns</param>
    public async Task ProgressAsync(string title, string text, Func<ProgressToast, Task> work)
    {
        await LaunchAsync<ProgressToast>(toast =>
        {
            toast.Add(x => x.Title, title);
            toast.Add(x => x.Text, text);
            toast.Add(x => x.Work, work);
        }, -1);
    }

    /// <summary>
    /// Launches a component inside a toast container and automatically hides it again
    /// </summary>
    /// <param name="onConfigure">Callback to configure the parameters of the component</param>
    /// <param name="hideDelayMs">Time in milliseconds until the modal should hide again. Use <b>-1</b> to disable this. Default: <b>5s</b></param>
    /// <typeparam name="T">Type of the component</typeparam>
    /// <returns>Reference to close the toast using <see cref="CloseAsync"/> or accessing the toast component instance itself</returns>
    public Task<ToastReference<T>> LaunchAsync<T>(Action<ComponentOptions<T>>? onConfigure = null, int hideDelayMs = 5000)
        where T : BaseToast
        => ToastLauncher.LaunchAsync(onConfigure, hideDelayMs);

    /// <summary>
    /// Closes the provided toast
    /// </summary>
    /// <param name="reference">Reference item of the toast to close</param>
    public Task CloseAsync(ToastReference reference)
        => ToastLauncher.CloseAsync(reference);

    internal void SetLauncher(ToastLauncher launcher) => ToastLauncher = launcher;
}