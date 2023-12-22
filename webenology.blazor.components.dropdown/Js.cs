using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.dropdown;
internal class Js : IAsyncDisposable
{
    private readonly DotNetObjectReference<Js> _ref;
    private ElementReference _el;
    private Action _onOutsideClick;
    private Action? _onInsideClick;
    public Lazy<Task<IJSObjectReference>> ModuleTask { get; set; }


    public Js(IJSRuntime jsRuntime)
    {
        ModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "/_content/webenology.blazor.components.dropdown/js/dropdown.js").AsTask());
        _ref = DotNetObjectReference.Create(this);
    }

    public async Task SetElement(ElementReference el)
    {
        _el = el;
        var value = await ModuleTask.Value;
        await value.InvokeVoidAsync("PreventEnterKey", _el);
    }

    public async Task ScrollToActive(int index, string behavior = "instant")
    {
        var value = await ModuleTask.Value;
        await value.InvokeVoidAsync("ScrollToActive", _el, index, behavior);
    }

    public async Task SetCursorToEnd()
    {
        var value = await ModuleTask.Value;
        await value.InvokeVoidAsync("CursorAtEnd", _el);
    }

    [JSInvokable]
    public void OnOutsideClick()
    {
        _onOutsideClick.Invoke();
    }

    [JSInvokable]
    public void OnInsideClick()
    {
        _onInsideClick?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        if (ModuleTask.IsValueCreated)
        {
            var module = await ModuleTask.Value;
            await module.DisposeAsync();
        }
    }
}