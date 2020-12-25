using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal interface IModalJsHelper : IAsyncDisposable
    {
        Task OpenModal(ElementReference el);
        Task CloseModal(ElementReference el);
    }

    internal class ModalJsHelper : BaseJsHelper, IModalJsHelper
    {
        private readonly IJSRuntime _jsRuntime;

        public ModalJsHelper(IJSRuntime jsRuntime) : base(jsRuntime, "./_content/webenology.blazor.components/js/webmodal.js")
        {
            _jsRuntime = jsRuntime;
        }

        public async Task OpenModal(ElementReference el)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("Open", el);
        }
        public async Task CloseModal(ElementReference el)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("Close", el);
        }
    }

}
