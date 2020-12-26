using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using SelectPdf;
using webenology.blazor.components.PdfComponent;
using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components
{
    public partial class PdfPageView
    {
        [CascadingParameter] private PdfPageModel _pdfPageModel { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        private PdfPageObject _pageObject = new();
        protected override void OnInitialized()
        {
            this.IfNullThrow(_pdfPageModel);
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _pdfPageModel.PdfPages.Add(_pageObject);
            }
            base.OnAfterRender(firstRender);
        }

        internal void AddTextElement(PdfPageTextBlock textBlock, PdfSizeUnit? sizeUnit)
        {
            var sU = sizeUnit ?? _pdfPageModel.PageSettingsModel.PdfSizeUnit;
            var x = textBlock.X.ToPoint(sU);
            var y = textBlock.Y.ToPoint(sU);
            var w = textBlock.Width.ToPoint(sU);
            var h = textBlock.Height.ToPoint(sU);
            
            var item = new PdfTextElement(x,y,w,h,textBlock.Text, );
            _pageObject.TextElements.Add(item);
        }
    }
}
