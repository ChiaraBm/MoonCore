namespace MoonCore.Blazor.FlyonUi.Files.Manager.Abstractions;

/// <summary>
/// Base definition of a file system operation
/// </summary>
public interface IFsOperation
{
    /// <summary>
    /// Name of the operation. Used for the UI of the file manager
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Icon to use in the UI. For a reference, look <see href="https://lucide.dev/icons">here</see>
    /// </summary>
    public string Icon { get; }
    
    /// <summary>
    /// Order index to overrule other potential compatibly operations. Higher value overrules lower value
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// Checks if an operation is compatible with the current file access and file manager settings.
    /// If it returns false the file manager will drop the implementation and won't use it
    /// </summary>
    /// <param name="access">File access used in the current instance</param>
    /// <param name="fileManager">File manager interface for the current instance</param>
    /// <returns>True if compatible otherwise false</returns>
    public bool CheckCompatability(IFsAccess access, IFileManager fileManager);
}