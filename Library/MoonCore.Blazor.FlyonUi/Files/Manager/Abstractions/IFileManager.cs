namespace MoonCore.Blazor.FlyonUi.Files.Manager.Abstractions;

/// <summary>
/// Use this interface in a file operation to invoke actions like <see cref="RefreshAsync"/> on the file manager
/// during the runtime of the operation
/// </summary>
public interface IFileManager
{
    /// <summary>
    /// Options the current file manager instance has been configured with
    /// </summary>
    public FileManagerOptions Options { get; }
    
    /// <summary>
    /// Refreshes the file list by fetching it from the <see cref="IFsAccess"/>
    /// </summary>
    /// <param name="silent">Whether to show a loading indicator while refreshing.
    /// If set to true the data will just appear for the user without indication if it actually loads</param>
    /// <returns></returns>
    public Task RefreshAsync(bool silent = false);
    
    /// <summary>
    /// Closes the current screen opened by an <see cref="IFsOpenOperation"/>
    /// </summary>
    /// <returns></returns>
    public Task CloseOpenScreenAsync();
}