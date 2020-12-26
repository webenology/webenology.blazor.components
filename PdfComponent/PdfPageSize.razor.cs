using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using webenology.blazor.components.PdfComponent.Models;

namespace webenology.blazor.components
{
    public partial class PdfPageSize
    {
        [CascadingParameter]
        private PdfPageSettings _pdfPageSettings { get; set; }
        [Parameter]
        public float Width { get; set; }
        [Parameter]
        public float Height { get; set; }
        [Parameter]
        public PdfSizeUnit? UnitSize { get; set; }

        protected override void OnInitialized()
        {
            this.IfNullThrow(_pdfPageSettings);
            _pdfPageSettings.AddPageSize(Width, Height, UnitSize);
            base.OnInitialized();
        }
    }
}
