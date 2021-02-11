using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    public interface IWebTextInputJsHelper
    {
        Task HighlightText(ElementReference el);
    }
    public class WebTextInputJsHelper : BaseJsHelper, IWebTextInputJsHelper
    {
        public WebTextInputJsHelper(IJSRuntime js): base(js, "./_content/webenology.blazor.components/js/webtextinput.js")
        {
        }

        public async Task HighlightText(ElementReference el)
        {
            var module = await ModuleTask.Value;
            await module.InvokeVoidAsync("selectText", el);
        }
    }
}
