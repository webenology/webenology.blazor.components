using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal interface IOutsideClickJsHelper : IAsyncDisposable
    {
        Task Register<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class;
        Task UnRegister<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class;
    }

    internal class OutsideClickJsHelper : IOutsideClickJsHelper
    {
        private readonly ILogger<OutsideClickJsHelper> _logger;
        public Lazy<Task<IJSObjectReference>> _moduleTask { get; set; }
        private IJSObjectReference? _classReference;

        public OutsideClickJsHelper(IJSRuntime jsRuntime, ILogger<OutsideClickJsHelper> logger)
        {
            _logger = logger;
            _moduleTask = new(() =>
                jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/webenology.blazor.components.OutsideClickComponent/js/outsideclick.js").AsTask());
        }

        public async Task Register<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class
        {
            var module = await _moduleTask.Value;
            _classReference = await module.InvokeAsync<IJSObjectReference>("CreateOutsideClick", el, instance);
        }

        public async Task UnRegister<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class
        {
            try
            {
                if (_classReference != null)
                    await _classReference.InvokeVoidAsync("unregister");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unregistering outside click.");
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_moduleTask.IsValueCreated)
                {
                    var module = await _moduleTask.Value;
                    await module.DisposeAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed when disposing outside click");
            }
            
        }
    }
}
