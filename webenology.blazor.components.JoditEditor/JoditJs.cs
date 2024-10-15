using System.Net.Security;
using System.Text;

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
    private IJSObjectReference moduleInstance;

    public JoditJs(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/webenology.blazor.components.JoditEditor/jodit.editor.js").AsTask());
    }

    public async ValueTask Setup(string id, Dictionary<string, string>? mergeTags,
        DotNetObjectReference<JoditEditor> dotNetRef)
    {
        var module = await moduleTask.Value;
        moduleInstance = await module.InvokeAsync<IJSObjectReference>("Instance", null);
        await moduleInstance.InvokeVoidAsync("Setup", id, mergeTags, dotNetRef);
    }

    public async ValueTask<string> GetText()
    {
        var str = new StringBuilder();
        var chunk = 0;
        while (true)
        {
            var results = await moduleInstance.InvokeAsync<string>("GetText", chunk);
            chunk++;
            if (string.IsNullOrEmpty(results))
                break;
            str.Append(results);
        }

        return str.ToString();
    }

    public async Task<string> GetHtml()
    {
        var data = await moduleInstance.InvokeAsync<IJSStreamReference>("GetHtml");
        await using var dataRef = await data.OpenReadStreamAsync();
        using var ms = new MemoryStream();
        await dataRef.CopyToAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
        return Encoding.UTF8.GetString(ms.ToArray());
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