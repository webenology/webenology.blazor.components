using Microsoft.JSInterop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public async Task Register(ElementReference el, Action onOutsideClick, Action? onInsideClick = null)
    {
        _onOutsideClick = onOutsideClick;
        _onInsideClick = onInsideClick;
        _el = el;
        var value = await ModuleTask.Value;
        await value.InvokeVoidAsync("Register", _el, _ref);
    }

    public async Task UnRegister()
    {
        var value = await ModuleTask.Value;
        await value.InvokeVoidAsync("UnRegister", _el, _ref);
    }

    public async Task ScrollToActive(string behavior = "instant")
    {
        var value = await ModuleTask.Value;
        await value.InvokeVoidAsync("ScrollToActive", _el, behavior);
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
            await UnRegister();
            var module = await ModuleTask.Value;
            await module.DisposeAsync();
        }
    }
}