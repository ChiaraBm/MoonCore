using MoonCore.Blazor.FlyonUI.Modals;

namespace MoonCore.Blazor.FlyonUI.Alerts;

public class AlertService
{
    private readonly ModalService ModalService;

    public AlertService(ModalService modalService)
    {
        ModalService = modalService;
    }

    public async Task Success(string title, string text)
        => await Launch<SuccessAlert>(title, text);

    public async Task Info(string title, string text)
        => await Launch<InfoAlert>(title, text);

    public async Task Warning(string title, string text)
        => await Launch<WarningAlert>(title, text);

    public async Task Error(string title, string text)
        => await Launch<ErrorAlert>(title, text);

    private async Task Launch<T>(string title, string text) where T : BaseModal
    {
        await ModalService.Launch<T>(buildAttr =>
        {
            buildAttr.Add("Title", title);
            buildAttr.Add("Text", text);
        }, size: "max-w-lg");
    }
}