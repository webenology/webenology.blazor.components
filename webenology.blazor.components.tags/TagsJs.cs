using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components.tags;
// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class TagsJs : IAsyncDisposable
{
    private readonly DotNetObjectReference<Tags> _dotNetRef;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public TagsJs(DotNetObjectReference<Tags> dotNetRef, IJSRuntime jsRuntime)
    {
        _dotNetRef = dotNetRef;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/webenology.blazor.components.tags/tags.js").AsTask());
    }

    public async Task PreventEnter(ElementReference el)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("subscribeOnKeyPress", _dotNetRef, el);
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
