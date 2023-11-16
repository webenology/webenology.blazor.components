using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components.ComboBoxComponents
{
    internal interface IComboBoxJsHelper
    {
        Task StopArrows(ElementReference el);
        Task ScrollTo(ElementReference el, int count, int pixelHeight);
    }
    internal class ComboBoxJsHelper : IComboBoxJsHelper, IAsyncDisposable
    {
        public readonly Lazy<Task<IJSObjectReference>> ModuleTask;
        public ComboBoxJsHelper(IJSRuntime jsRuntime)
        {
            ModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/webenology.blazor.components.ComboBoxComponents/js/combobox.js").AsTask());
        }

        public async Task StopArrows(ElementReference el)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("StopArrows", el);
        }

        public async Task ScrollTo(ElementReference el, int count, int pixelHeight)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("ScrollTo", el, count, pixelHeight);
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
