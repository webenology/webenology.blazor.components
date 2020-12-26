using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components
{
    public partial class PdfPageMarginModel
    {
        [CascadingParameter]
        private PdfPageSettings _pdfPageSettings { get; set; }
        [Parameter]
        public float Left { get; set; }
        [Parameter]
        public float Right { get; set; }
        [Parameter]
        public float Top { get; set; }
        [Parameter]
        public float Bottom { get; set; }
        [Parameter]
        public PdfSizeUnit? UnitSize { get; set; }
        
        protected override void OnInitialized()
        {
            this.IfNullThrow(_pdfPageSettings);

            base.OnInitialized();
        }
    }
}
