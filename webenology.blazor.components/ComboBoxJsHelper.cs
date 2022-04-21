using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal interface IComboBoxJsHelper
    {
        Task StopArrows(ElementReference el);
        Task ScrollTo(ElementReference el, int count, int pixelHeight);
    }
    internal class ComboBoxJsHelper : BaseJsHelper, IComboBoxJsHelper
    {
        public ComboBoxJsHelper(IJSRuntime jsRuntime) : base(jsRuntime, "./_content/webenology.blazor.components/js/combobox.js")
        {
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
    }
}
