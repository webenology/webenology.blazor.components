using Microsoft.JSInterop;

namespace webenology.blazor.components.JoditEditor;
// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class JoditJs : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public JoditJs(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/webenology.blazor.components.JoditEditor/jodit.editor.js").AsTask());
    }

    public async ValueTask Setup(string id, Dictionary<string, string>? mergeTags)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("Setup", id, mergeTags);
    }

    public async ValueTask<string> GetText()
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<string>("GetText", null);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
