using MoonCore.Blazor.FlyonUi.Modals;

namespace MoonCore.Blazor.FlyonUi.Alerts;

public class AlertService
{
    private readonly ModalService ModalService;

    public AlertService(ModalService modalService)
    {
        ModalService = modalService;
    }

    /// <summary>
    /// Launches an instance of the default success modal with the provided content
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    public Task SuccessAsync(string title, string text)
        => LaunchInternalAsync<SuccessAlert>(title, text);

    /// <summary>
    /// Launches an instance of the default info modal with the provided content
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    public Task InfoAsync(string title, string text)
        => LaunchInternalAsync<InfoAlert>(title, text);

    /// <summary>
    /// Launches an instance of the default warning modal with the provided content
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    public Task WarningAsync(string title, string text)
        => LaunchInternalAsync<WarningAlert>(title, text);

    /// <summary>
    /// Launches an instance of the default error modal with the provided content
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    public Task ErrorAsync(string title, string text)
        => LaunchInternalAsync<ErrorAlert>(title, text);

    /// <summary>
    /// Launches a confirmation modal for a dangerous operation
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    /// <param name="confirmAction">Callback which will be invoked when the user confirms the action</param>
    public async Task ConfirmDangerAsync(string title, string text, Func<Task> confirmAction)
    {
        await ModalService.LaunchAsync<ConfirmDangerAlert>(options =>
        {
            options.Add(x => x.Title, title);
            options.Add(x => x.Text, text);
            options.Add(x => x.ConfirmAction, confirmAction);
        });
    }

    private async Task LaunchInternalAsync<T>(string title, string text) where T : BaseModal
    {
        await ModalService.LaunchAsync<T>(parameters =>
        {
            parameters.Add("Title", title);
            parameters.Add("Text", text);
        });
    }
}