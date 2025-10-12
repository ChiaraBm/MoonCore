using MoonCore.Blazor.FlyonUi.Files.Manager.Abstractions;
using MoonCore.Blazor.FlyonUi.Files.Manager.Modals;
using MoonCore.Blazor.FlyonUi.Modals;
using MoonCore.Helpers;

namespace MoonCore.Blazor.FlyonUi.Files.Manager.Operations;

public class CreateFolderOperation : IToolbarOperation
{
    public string Name => "Folder";
    public string Icon => "icon-folder";
    public string ToolbarCss => "btn-accent";
    public int Order => 0;

    private readonly ModalService ModalService;

    public CreateFolderOperation(ModalService modalService)
    {
        ModalService = modalService;
    }

    /// <inheritdoc />
    public bool CheckCompatability(IFsAccess access, IFileManager fileManager)
        => true;

    /// <inheritdoc />
    public async Task ExecuteAsync(string workingDir, IFsAccess fsAccess, IFileManager fileManager)
    {
        await ModalService.LaunchAsync<CreateDirectoryModal>(modal =>
        {
            modal.Add(x => x.OnSubmit, async name =>
            {
                await fsAccess.CreateDirectoryAsync(UnixPath.Combine(
                    workingDir,
                    name
                ));

                await fileManager.RefreshAsync();
            });
        });
    }
}