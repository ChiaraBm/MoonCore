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
    public async Task SuccessAsync(string title, string text)
    {
        await ModalService.LaunchAsync<SuccessAlert>(alert =>
        {
            alert.Title = title;
            alert.Text = text;
        });
    }

    /// <summary>
    /// Launches an instance of the default info modal with the provided content
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    public async Task InfoAsync(string title, string text)
    {
        await ModalService.LaunchAsync<InfoAlert>(alert =>
        {
            alert.Title = title;
            alert.Text = text;
        });
    }

    /// <summary>
    /// Launches an instance of the default warning modal with the provided content
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    public async Task WarningAsync(string title, string text)
    {
        await ModalService.LaunchAsync<WarningAlert>(alert =>
        {
            alert.Title = title;
            alert.Text = text;
        });
    }

    /// <summary>
    /// Launches an instance of the default error modal with the provided content
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    public async Task ErrorAsync(string title, string text)
    {
        await ModalService.LaunchAsync<ErrorAlert>(alert =>
        {
            alert.Title = title;
            alert.Text = text;
        });
    }

    /// <summary>
    /// Launches a confirmation modal for a dangerous operation
    /// </summary>
    /// <param name="title">Title of the modal</param>
    /// <param name="text">Content of the modal</param>
    /// <param name="confirmAction">Callback which will be invoked when the user confirms the action</param>
    public async Task ConfirmDangerAsync(string title, string text, Func<Task> confirmAction)
    {
        await ModalService.LaunchAsync<ConfirmDangerAlert>(alert =>
        {
            alert.Title = title;
            alert.Text = text;
            alert.ConfirmAction = confirmAction;
        });
    }
}