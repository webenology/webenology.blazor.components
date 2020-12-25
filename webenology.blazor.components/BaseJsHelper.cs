using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal abstract class BaseJsHelper : IAsyncDisposable
    {
        public readonly Lazy<Task<IJSObjectReference>> ModuleTask;
        protected BaseJsHelper(IJSRuntime jsRuntime, string url)
        {
            ModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", url).AsTask());

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
}
