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
        Task Setup<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class;
        Task SetFocus(ElementReference el);
        Task RemoveInstance(ElementReference el);
    }

    internal class OutsideClickJsHelper : BaseJsHelper, IOutsideClickJsHelper
    {
        public OutsideClickJsHelper(IJSRuntime jsRuntime) : base(jsRuntime, "./_content/webenology.blazor.components/js/outsideclick.js")
        {
        }

        public async Task Setup<TRef>(ElementReference el, DotNetObjectReference<TRef> instance) where TRef : class
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("Setup", el, instance);
        }

        public async Task SetFocus(ElementReference el)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("SetFocusInAttr", el);
        }

        public async Task RemoveInstance(ElementReference el)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("RemoveInstance", el);
        }

    }
}
