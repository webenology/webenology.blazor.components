using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using webenology.blazor.components.PdfComponent;
using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components
{
    public partial class WebPdfViewer<TDataSource> : IDisposable
    {
        [Parameter]
        public TDataSource DataSource { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Inject]
        private IWebPdfViewerJsHelper js { get; set; }
        [Inject]
        private IPdfBuilderHelper _pdfBuilderHelper { get; set; }

        private PdfPageModel _pdfPageModel = new PdfPageModel();
        private string _url;
        private string _base64;
        private MemoryStream _pdfStream = new MemoryStream();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadData();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData()
        {
            _base64 = _pdfBuilderHelper.BuildPdfToBase64(_pdfPageModel);
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _pdfStream?.Dispose();
        }
    }
}
