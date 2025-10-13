using Microsoft.JSInterop;

namespace MoonCore.Blazor.FlyonUi.Ace;

/// <summary>
/// Interop service to communicate with the moonCore js component
/// creating and managing ace editor instances
/// </summary>
public class CodeEditorService
{
    private readonly IJSRuntime JsRuntime;

    /// <summary>
    /// Creates a service instance to communicate with the moonCore js component
    /// </summary>
    /// <param name="jsRuntime">Javascript runtime used to communicate</param>
    public CodeEditorService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    /// <summary>
    /// Attaches an ace editor instance with the provided options to a dom element
    /// </summary>
    /// <param name="id">Id of the element to attach to</param>
    /// <param name="options">Options to configure the ace editor instance with</param>
    public async Task AttachAsync(string id, CodeEditorOptions options)
    {
        await JsRuntime.InvokeVoidAsync("moonCore.codeEditor.attach", id, options);
    }

    /// <summary>
    /// Updates the options of an existing ace editor instance
    /// </summary>
    /// <param name="id">Id of the ace editor</param>
    /// <param name="options">New options to update the editor with</param>
    public async Task UpdateOptionsAsync(string id, CodeEditorOptions options)
    {
        await JsRuntime.InvokeVoidAsync("moonCore.codeEditor.updateOptions", id, options);
    }

    /// <summary>
    /// Retrieves the string content of the ace editor instance
    /// </summary>
    /// <param name="id">Id of the editor</param>
    /// <returns>String content of the editor</returns>
    public async Task<string> GetValueAsync(string id)
    {
        return await JsRuntime.InvokeAsync<string>("moonCore.codeEditor.getValue", id);
    }

    /// <summary>
    /// Destroys an ace editor instance defined by the id
    /// </summary>
    /// <param name="id">Id of the ace editor instance</param>
    public async Task DestroyAsync(string id)
    {
        await JsRuntime.InvokeVoidAsync("moonCore.codeEditor.destroy", id);
    }
}