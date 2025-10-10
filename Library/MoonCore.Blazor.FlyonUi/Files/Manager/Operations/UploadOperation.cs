using MoonCore.Blazor.FlyonUi.Files.Manager.Abstractions;
using MoonCore.Blazor.FlyonUi.Files.Manager.Modals;
using MoonCore.Blazor.FlyonUi.Modals;
using MoonCore.Blazor.FlyonUi.Toasts;
using MoonCore.Common;
using MoonCore.Helpers;

namespace MoonCore.Blazor.FlyonUi.Files.Manager.Operations;

public class UploadOperation : IToolbarOperation
{
    /// <inheritdoc />
    public string Name => "Upload";

    /// <inheritdoc />
    public string Icon => "icon-hard-drive-upload";

    /// <inheritdoc />
    public int Order => 100;

    /// <inheritdoc />
    public string ToolbarCss => "btn-primary";

    private readonly ModalService ModalService;

    public UploadOperation(ModalService modalService)
    {
        ModalService = modalService;
    }

    /// <inheritdoc />
    public bool CheckCompatability(IFsAccess access, IFileManager fileManager)
        => true;

    /// <inheritdoc />
    public async Task ExecuteAsync(string workingDir, IFsAccess access, IFileManager fileManager)
    {
        await ModalService.LaunchAsync<UploadModal>(parameters =>
        {
            parameters["MaxSize"] = access is ICombineAccess
                ? fileManager.Options.UploadLimit
                : fileManager.Options.WriteLimit;

            parameters["OnCompleted"] = async () =>
            {
                await fileManager.RefreshAsync(silent: true);
            };

            parameters["Callback"] = async (string path, Stream stream, Func<int, Task> onProgress) =>
            {
                var destination = UnixPath.Combine(workingDir, path);
                
                if (stream.Length <= fileManager.Options.WriteLimit)
                {
                    await access.WriteAsync(destination, stream);
                }
                else
                {
                    if(access is not ICombineAccess combineAccess)
                        return; // Should never be valid because cause the max size should prevent it

                    var uploadId = Random.Shared.Next(0, 10324);
                    var uploadTmpFolder = $"/.upload{uploadId}";

                    await access.CreateDirectoryAsync(uploadTmpFolder);

                    var counter = 0;
                    var paths = new List<string>();
                    
                    while (stream.Position < stream.Length)
                    {
                        var subStream = new SubStream(stream, fileManager.Options.WriteLimit);

                        var itemPath = UnixPath.Combine(uploadTmpFolder, $"{counter}.bin");
                        
                        await access.WriteAsync(itemPath, subStream);
                        
                        var percent = (int)Math.Round(stream.Position / (float)stream.Length * 100f);
                        await onProgress.Invoke(percent);
                        
                        paths.Add(itemPath);
                        counter++;
                    }

                    await combineAccess.CombineAsync(destination, paths.ToArray());
                    await access.DeleteAsync(uploadTmpFolder);
                }
            };
        });
    }
}