using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    internal interface IWebPdfViewerJsHelper
    {
        Task<string> GetPdfUrl(string base64Pdf);
    }

    internal class WebPdfViewerJsHelper : BaseJsHelper, IWebPdfViewerJsHelper
    {
        public WebPdfViewerJsHelper(IJSRuntime jsRuntime) : base(jsRuntime, "./_content/webenology.blazor.components/js/webpdfviewer.js")
        {
        }

        public async Task<string> GetPdfUrl(string base64Pdf)
        {
            var module = await ModuleTask.Value;
            return await module.InvokeAsync<string>("getBlobFromPdf", base64Pdf);
        }
    }
}
