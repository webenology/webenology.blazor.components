using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal interface IOutsideClickJsHelper
    {
        Task Register<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class;
        Task UnRegister<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class;
    }

    internal class OutsideClickJsHelper : IOutsideClickJsHelper, IAsyncDisposable
    {
        public Lazy<Task<IJSObjectReference>> _moduleTask { get; set; }

        public OutsideClickJsHelper(IJSRuntime jsRuntime)
        {
            _moduleTask = new(() =>
                jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/webenology.blazor.components.OutsideClickComponent/js/outsideclick.js").AsTask());
        }

        public async Task Register<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("Register", el, instance);
        }

        public async Task UnRegister<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("UnRegister", el, instance);
        }

        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
