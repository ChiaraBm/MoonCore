using MoonCore.Blazor.FlyonUi.Toasts.Components;

namespace MoonCore.Blazor.FlyonUi.Toasts;

public class ToastService
{
    private ToastLauncher ToastLauncher;

    /// <summary>
    /// Launches a success toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public async Task SuccessAsync(string title, string text = "")
    {
        await LaunchAsync<SuccessToast>(toast =>
        {
            toast.Title = title;
            toast.Text = text;
        });
    }

    /// <summary>
    /// Launches an info toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public async Task InfoAsync(string title, string text = "")
    {
        await LaunchAsync<InfoToast>(toast =>
        {
            toast.Title = title;
            toast.Text = text;
        });
    }

    /// <summary>
    /// Launches a warning toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public async Task WarningAsync(string title, string text = "")
    {
        await LaunchAsync<WarningToast>(toast =>
        {
            toast.Title = title;
            toast.Text = text;
        });
    }

    /// <summary>
    /// Launches an error toast with the provided text
    /// </summary>
    /// <param name="title">Title of the toast</param>
    /// <param name="text"><b>Optional:</b> Content of the toast</param>
    /// <returns></returns>
    public async Task ErrorAsync(string title, string text = "")
    {
        await LaunchAsync<ErrorToast>(toast =>
        {
            toast.Title = title;
            toast.Text = text;
        });
    }

    public async Task ProgressAsync(string title, string defaultText, Func<ProgressToast, Task> work)
    {
        await LaunchAsync<ProgressToast>(toast =>
        {
            toast.Title = title;
            toast.Text = defaultText;
            toast.Work = work;
        }, -1);
    }

    /// <summary>
    /// Launches a component inside a toast container and automatically hides it again
    /// </summary>
    /// <param name="onConfigure">Callback to configure the parameters of the component</param>
    /// <param name="hideDelayMs">Time in milliseconds until the modal should hide again. Use <b>-1</b> to disable this. Default: <b>5s</b></param>
    /// <typeparam name="T">Type of the component</typeparam>
    /// <returns>Reference item to close the toast using <see cref="CloseAsync"/></returns>
    public Task<ToastReference<T>> LaunchAsync<T>(Action<T>? onConfigure = null, int hideDelayMs = 5000)
        where T : BaseToast
        => ToastLauncher.LaunchAsync<T>(onConfigure, hideDelayMs);

    /// <summary>
    /// Closes the provided toast
    /// </summary>
    /// <param name="reference">Reference item of the toast to close</param>
    public Task CloseAsync(ToastReference reference)
        => ToastLauncher.CloseAsync(reference);

    internal void SetLauncher(ToastLauncher launcher) => ToastLauncher = launcher;
}