using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class PdfPageTextBlock
    {
        [CascadingParameter]
        private PdfPageView _pdfPageView { get; set; }
        [Parameter]
        public float X { get; set; }
        [Parameter]
        public float Y { get; set; }
        [Parameter]
        public float Width { get; set; }
        [Parameter]
        public float Height { get; set; }
        [Parameter]
        public string Text { get; set; }

        protected override void OnInitialized()
        {
            this.IfNullThrow(_pdfPageView);

            base.OnInitialized();
        }
    }
}
